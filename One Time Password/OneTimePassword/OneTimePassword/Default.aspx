<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="OneTimePassword.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            text-align: left;
        }
        .style3
        {
            text-align: center;
            color: #6699FF;
            font-weight: bold;
        }
        .style4
        {
            color: #000099;
        }
        .style5
        {
            text-align: right;
            width: 315px;
        }
        .style6
        {
            width: 315px;
            text-align: right;
        }
        .style7
        {
            width: 315px;
            text-align: center;
            height: 30px;
        }
        .style8
        {
            height: 30px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
  <div>
  <div id= "div_registerdata" runat="server">
    <div id="div_registerprocess">
    <table class="style1">
        <tr>
            <td class="style3" colspan="2">
                <h2 class="style4">
                    One Time Password Implementation
                </h2>
            </td>
        </tr>
        <tr>
            <td class="style2" colspan="2">
                First Register to Server and click below button and get Smart card number. 
                if already Register
                <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click">click here</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td class="style5">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style5">
                &nbsp;</td>
            <td>
                <asp:Button ID="btnregister" runat="server" onclick="btnregister_Click" 
                    Text="Register Now " Height="30px" Width="146px" />
            </td>
        </tr>
        </table>
    </div>
    <div id="div_showseed" runat="server"  visible="false">
    
        <table class="style1">
            <tr>
                <td class="style6">
                    Smart Card (Seed): </td>
                <td>
                <asp:Label ID="lblSeed" runat="server"></asp:Label>
            &nbsp; </td>
            </tr>
            <tr>
                <td class="style7">
                    </td>
                <td class="style8">
                    &nbsp;<asp:Button ID="btngetsecrete" runat="server" Text="Go farword " 
                        onclick="btngetsecrete_Click" />
                </td>
            </tr>
            </table>
    
    </div>
    <div id="id_shownounceandmessage" runat="server" visible="false">
    
        <table class="style1">
            <tr>
                <td class="style6">
                    Secrete code(Nounce 
                    D and N value) : </td>
                <td>
                    <asp:Label ID="lblshownounce_fruser" runat="server"></asp:Label>
                &nbsp;and N:
                    <asp:Label ID="lbl_clientN" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style6">
                    &nbsp;</td>
                <td>
                    <asp:Label ID="lblshow_correctcode" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style6">
                    &nbsp;</td>
                <td>
                    <asp:Button ID="btnlogin" runat="server" Text=" Process farword to authenticate " 
                        Visible="False" onclick="btnlogin_Click" />
                </td>
            </tr>
        </table>
    
    </div>
   <div id="div_usrnamePass" runat= "server" visible="false">
    
        <table class="style1">
            <tr>
                <td class="style6">
                    Enter Username : </td>
                <td>
                    <asp:TextBox ID="txtusername" runat="server" Width="220px" ValidationGroup="login"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"  ValidationGroup="login"
                         ControlToValidate="txtusername" ErrorMessage="* Required field cannot be left blank."
                  Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style6">
                    Password : </td>
                <td>
                    <asp:TextBox ID="txtpassword" runat="server" TextMode="Password" Width="220px" 
                        CausesValidation="True" ToolTip="password " ValidationGroup="login"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"  ValidationGroup="login"
                          ControlToValidate="txtpassword" ErrorMessage="* Required field cannot be left blank."
                           ForeColor="Red"  Display="Dynamic">
                        </asp:RequiredFieldValidator>

                </td>
            </tr>
            <tr>
                <td class="style6">
                    &nbsp;</td>
                <td>
                    <asp:Label ID="lblshow_register_errormsg" runat="server"></asp:Label>

                </td>
            </tr>
            <tr>
                <td class="style6">
                    &nbsp;</td>
                <td>
                    <asp:Button ID="btn_savepass" runat="server" Text="Log in" 
                        onclick="btn_savepass_Click" ValidationGroup="login" />
                </td>
            </tr>
        </table>
    
   </div>
   </div>
 <div id="div_loginuser" runat="server" visible="false">
 
        <table class="style1">
            <tr>
                <td class="style6">
                    Enter Username : </td>
                <td>
                    <asp:TextBox ID="txtlogin_username" runat="server" Width="220px"></asp:TextBox>
                    
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="login_auth"
    ControlToValidate="txtlogin_username" ErrorMessage="* Required field cannot be left blank."
    Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style6">
                    Password : </td>
                <td>
                    <asp:TextBox ID="txtlogin_password" runat="server" TextMode="Password" 
                        Width="220px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="login_auth"
    ControlToValidate="txtlogin_password" ErrorMessage="* Required field cannot be left blank."
    Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td class="style6">
                    Enter Seed: </td>
                <td>
                    <asp:TextBox ID="txtlogin_seed" runat="server" Width="220px"></asp:TextBox>
                    
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="login_auth"
    ControlToValidate="txtlogin_seed" ErrorMessage="* Required field cannot be left blank."
    Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style6">
                    &nbsp;</td>
                <td>
                    <asp:Label ID="lbllogin_messageshow" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style6">
                    &nbsp;</td>
                <td>
                    <asp:Button ID="btn_login_auth" runat="server" Text="login User" 
                        onclick="btn_login_auth_Click" ValidationGroup="login_auth" 
                       />
                </td>
            </tr>
        </table>
    
 </div>
   </div>
    </form>
</body>
</html>
