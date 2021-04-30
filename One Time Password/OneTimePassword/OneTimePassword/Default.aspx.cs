using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace OneTimePassword
{
    public partial class Default : System.Web.UI.Page
    {
        //string seed, nounce, N, hasvalue;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnregister_Click(object sender, EventArgs e)
        {
            Label lblnounce = new Label();
            Label lblXor = new Label();
            Label lblhashD0 = new Label();
            lblSeed.Text =Convert.ToString( getSeed()); 
            lblnounce.Text = getNounce().ToString();
            Session["seed"] = lblSeed.Text;
            lblXor.Text = GetXOR(lblnounce.Text,lblSeed.Text);
            Session["seedXORnounce"] = lblXor.Text;
            lblhashD0.Text = GetHash( lblnounce.Text,lblSeed.Text);
            Session["hD"] = lblhashD0.Text;
            div_showseed.Visible = true;
        }
        protected string GetHash(string password, string salt)
         {
                 MD5 md5 = new MD5CryptoServiceProvider();
                 byte [] digest = md5.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
                 string base64digest = Convert.ToBase64String(digest, 0, digest.Length);
                 return base64digest.Substring(0, base64digest.Length-2);
         }
        protected string GetXOR( string input,string key ) 
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
                sb.Append((char)(input[i] ^ key[(i % key.Length)]));
            String result = sb.ToString();

            return result;
        }
        protected int getSeed() 
        {
            Random r = new Random();
            int x = r.Next(1000, 5000);//Max range
            return x;        
        }
        protected int getNounce()
        {
            Random n = new Random();
            int nounce = n.Next(5001, 9990);//Max range
            return nounce;
        }
        protected int getN()
        {
            Random n = new Random();
            int Num = n.Next(2, 10);//Max range
            return Num;
         }
       
        protected void btngetsecrete_Click(object sender, EventArgs e)
        {
            try
            {
                string lblN;
                string newhash;
                lblN = getN().ToString();
                Session["N"] = lblN;
                lbl_clientN.Text = lblN;
                string xornounce = Session["seedXORnounce"].ToString();
                lblshownounce_fruser.Text = GetXOR(lblSeed.Text, xornounce);
                newhash = GetHash(lblshownounce_fruser.Text, lblSeed.Text);
                string oldhash = Session["hD"].ToString();
                if (newhash.Length == oldhash.Length)
                {
                    int i = 0;
                    while ((i < newhash.Length) && (newhash[i] == oldhash[i]))
                    {
                        i += 1;
                    }
                    if (i == newhash.Length)
                    {
                        lblshow_correctcode.Text = "secrete value is correct and secure";
                        lblshow_correctcode.ForeColor = System.Drawing.Color.Green;
                        btnlogin.Visible = true;
                    }
                    else
                    {
                        lblshow_correctcode.Text = "Value is not correct!";
                        lblshow_correctcode.ForeColor = System.Drawing.Color.Red;
                    }
                }
                id_shownounceandmessage.Visible = true;
            }
            catch(Exception x){}
        }

        protected void btnlogin_Click(object sender, EventArgs e)
        {
            div_usrnamePass.Visible = true;
        }

        protected void btn_savepass_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtusername.Text.Length >= 6 && txtpassword.Text.Length >= 6)
                {
                    string KxorSeed = GetXOR(txtpassword.Text, lblSeed.Text);

                    Int64 N = Convert.ToInt64(lbl_clientN.Text);
                    for (int i = 0; i < N; i++)
                    {
                        KxorSeed = GetHash(KxorSeed, lblSeed.Text);

                    }
                    string PXorNounce = GetXOR(KxorSeed, lblshownounce_fruser.Text);
                    //////////save in database serverside:
                    Database_conn(PXorNounce, Session["N"].ToString(), Session["seedXORnounce"].ToString(), Session["seed"].ToString());
                    div_loginuser.Visible = true;
                    div_registerdata.Visible = false;
                }
                else
                {
                    lblshow_register_errormsg.Text = "User name and password length must be more then 6 characters";
                    lblshow_register_errormsg.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch( Exception x){}
        }
        protected void Database_conn( string hashdpass, string N, string nounce, string seed)
        {
            try
            {
                SqlConnection cn = new SqlConnection("Data Source=localhost;Initial Catalog=Password_sc;Integrated Security=True");
                if (cn.State.ToString() == "Closed")
                {
                    cn.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SELECT UserName FROM tble_oneTimepass WHERE UserName='" + txtusername.Text+ "'", cn);
                SqlDataReader usernameRdr = null;
                string user = null;
                usernameRdr = cmd1.ExecuteReader();
                while (usernameRdr.Read())
                {
                    user = usernameRdr["UserName"].ToString();
                }
                cn.Close();
                if (user == null)
                {
                    //hashdpass = GetXOR(hashdpass, lblshownounce_fruser.Text);
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO tble_oneTimepass  values('" + txtusername.Text + "','" + hashdpass + "','" + N + "','" + nounce + "','" + seed + "','" + Session["hD"].ToString() + "','"+ Convert.ToInt32( N)+"')", cn);
                     cmd.ExecuteNonQuery();
                     cn.Close();
                     Response.Write("<script>alert('Register successful');</script>");        
                }
                else
                {
                    Response.Write("<script>alert('user name already exist!');</script>");
                }
            }
            catch (Exception xe)
            {
                //  MessageBox.Show(xe.Message);
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            div_loginuser.Visible = true;
            div_registerdata.Visible = false;
        }
        protected void getuser_Data() 
        {
            try
            {
                SqlConnection cn = new SqlConnection("Data Source=localhost;Initial Catalog=Password_sc;Integrated Security=True");
                if (cn.State.ToString() == "Closed")
                {
                    cn.Open();
                }
                SqlCommand cmd1 = new SqlCommand("SELECT UserId,UserName,P0,Nounce,N, Count,HashD FROM tble_oneTimepass WHERE UserName='" + txtlogin_username.Text + "'", cn);
                SqlDataReader usernameRdr = null;
                string user = null;
                usernameRdr = cmd1.ExecuteReader();
                while (usernameRdr.Read())
                {
                    Session["userid"] = usernameRdr["UserId"].ToString();
                    Session["user"] = usernameRdr["UserName"].ToString();
                    Session["pass"] = usernameRdr["P0"].ToString();
                     Session["seedXORnounce"]= usernameRdr["Nounce"].ToString();
                     Session["N"] = usernameRdr["N"].ToString();
                    Session["hD"] = usernameRdr["HashD"].ToString();
                    Session["count"] = usernameRdr["Count"].ToString();
                }
            }
            catch (Exception xe)
            {
                //  MessageBox.Show(xe.Message);
            }
 
        
        }
        protected string check_hashcompare(string oldhash, string newhashnounce) 
        {
            if (newhashnounce.Length == oldhash.Length)
            {
                int i = 0;
                while ((i < newhashnounce.Length) && (newhashnounce[i] == oldhash[i]))
                {
                    i += 1;
                }
                if (i == newhashnounce.Length)
                {
                    string retn = "true";
                    return retn;
                }
                else
                {
                    return "false";
                }
            }
            else 
            {
                return "false";
            }
         
        }
        protected void btn_login_auth_Click(object sender, EventArgs e)
        {
            try
            {
                getuser_Data();
                string Nounce_t = Session["seedXORnounce"].ToString();
                Nounce_t = GetXOR(Nounce_t, txtlogin_seed.Text);
                string newhashnounce = GetHash(Nounce_t, txtlogin_seed.Text);

                string oldhash = Session["hD"].ToString();
                string check = check_hashcompare(oldhash, newhashnounce);
                if (check == "true")
                {

                    string KxorSeed = GetXOR(txtlogin_password.Text, txtlogin_seed.Text);

                    Int64 N = Convert.ToInt64(Session["N"].ToString());
                    for (int i = 0; i < N; i++)
                    {
                        KxorSeed = GetHash(KxorSeed, txtlogin_seed.Text);

                    }
                    string PXorNounce = GetXOR(KxorSeed, Nounce_t);
                    string checkpass = check_hashcompare(Session["pass"].ToString(), PXorNounce);
                    if (checkpass == "true")
                    {
                        lbllogin_messageshow.Text = "password is correct";  ///////////////call new password update db
                        ////////////////////////////////////////////////////////    again hash function n time and update previous passowrd:
                        KxorSeed = GetXOR(txtlogin_password.Text, txtlogin_seed.Text);
                        N = Convert.ToInt64(Session["N"].ToString());
                        for (int i = 0; i < N; i++)
                        {
                            KxorSeed = GetHash(KxorSeed, txtlogin_seed.Text);

                        }
                        string newnounce = getNounce().ToString();
                        PXorNounce = GetXOR(KxorSeed, newnounce);
                        string newseedXORnounce = GetXOR(newnounce, txtlogin_seed.Text);
                        ///new nounce and password
                        ///get hash password
                        newnounce = GetHash(newnounce, txtlogin_seed.Text); //new hashnounce value for next use
                        Session["hD"] = newnounce;
                        int count = Convert.ToInt32(Session["count"].ToString());
                        if (count != 0)
                        {
                            count = count - 1;
                            update_Data_newpass(PXorNounce, Convert.ToInt32(Session["userid"].ToString()), newseedXORnounce, count, Session["hD"].ToString());
                        }
                        else
                        {
                            lbllogin_messageshow.Text = "login time expire register again!";
                            lbllogin_messageshow.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
                else
                {
                    lbllogin_messageshow.Text = "Value is not correct!";
                    lbllogin_messageshow.ForeColor = System.Drawing.Color.Red;
                }
            }catch(Exception x){}
        }
        protected void update_Data_newpass( string hashpass, int uid,  string nounce, int count, string hashvaluenounce)
        {
            try
            {
                SqlConnection cn = new SqlConnection("Data Source=localhost;Initial Catalog=Password_sc;Integrated Security=True");
                if (cn.State.ToString() == "Closed")
                {
                    cn.Open();
                }
                SqlCommand cmd1 = new SqlCommand("update tble_oneTimepass set P0='"+hashpass+"',Nounce='"+nounce+"',count='"+count+"', HashD='"+hashvaluenounce+"' where UserId='" + Convert.ToInt32( Session["userid"])+ "'", cn);
                cmd1.ExecuteNonQuery();
                cn.Close();
                Response.Write("<script>alert('authenticate successfully');</script>");
                Response.Redirect("userprofile.aspx");
            }
            catch (Exception xe)
            {
                //  MessageBox.Show(xe.Message);
            }


        }


    }
}