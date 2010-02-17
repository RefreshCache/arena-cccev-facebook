<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FBPublishRegistration.ascx.cs" Inherits="ArenaWeb.UserControls.Custom.Cccev.Facebook.FBPublishRegistration" %>
<asp:ScriptManagerProxy ID="smpScripts" runat="server" />
<script type="text/javascript">
    initFacebook('<%= GetApiKey() %>', '<%= GetReceiverPath() %>');
</script>
<asp:PlaceHolder ID="phFBPublish" runat="server" />