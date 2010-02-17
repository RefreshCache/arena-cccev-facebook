/**********************************************************************
* Description:  Publishes registration information to Facebook
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 2/16/2010
*
* $Workfile: FBPublishRegistration.ascx.cs $
* $Revision: 2 $
* $Header: /trunk/Arena/UserControls/Custom/Cccev/Facebook/FBPublishRegistration.ascx.cs   2   2010-02-17 09:53:02-07:00   JasonO $
*
* $Log: /trunk/Arena/UserControls/Custom/Cccev/Facebook/FBPublishRegistration.ascx.cs $
*  
*  Revision: 2   Date: 2010-02-17 16:53:02Z   User: JasonO 
*  Fixing jquery include issues. 
*  
*  Revision: 1   Date: 2010-02-17 00:52:05Z   User: JasonO 
**********************************************************************/

using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using Arena.Custom.Cccev.DataUtils;
using Arena.Event;
using Arena.Portal;
using Microsoft.Security.Application;

namespace ArenaWeb.UserControls.Custom.Cccev.Facebook
{
    public partial class FBPublishRegistration : PortalControl
    {
        [PageSetting("Event Detail Page", "The page that should be used to display event details.", true)]
        public string EventDetailPageSetting { get { return Setting("EventDetailPage", "", true); } }

        private Registration registration;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                smpScripts.Scripts.Add(new ScriptReference(string.Format("~/{0}", BasePage.JQUERY_INCLUDE)));
                smpScripts.Scripts.Add(new ScriptReference(GetFacebookScriptPath()));
                smpScripts.Scripts.Add(new ScriptReference("~/UserControls/Custom/Cccev/Facebook/js/facebook.js"));

                if (!string.IsNullOrEmpty(Request.QueryString["rgtn"]))
                {
                    registration = new Registration(new Guid(Request.QueryString["rgtn"]));
                    if (registration.RegistrationId != -1)
                    {
                        ShowFBPublish();
                    }
                    else
                    {
                        ShowError("The Registration you are requesting does not exist.");
                    }
                }
            }
        }

        protected string GetApiKey()
        {
            return CurrentOrganization.Settings["Facebook.APIKey"];
        }

        protected string GetReceiverPath()
        {
            switch (Page.Request.Url.Scheme)
            {
                case "https":
                    return CurrentOrganization.Settings["Facebook.SSLReceiverPath"];
                default:
                    return CurrentOrganization.Settings["Facebook.ReceiverPath"];
            }
        }

        private string GetFacebookScriptPath()
        {
            switch (Page.Request.Url.Scheme)
            {
                case "https":
                    return "https://ssl.connect.facebook.com/js/api_lib/v0.4/FeatureLoader.js.php";
                default:
                    return "http://static.ak.connect.facebook.com/js/api_lib/v0.4/FeatureLoader.js.php";
            }
        }

        private void ShowError(string error)
        {
            phFBPublish.Controls.Add(new LiteralControl(string.Format("<span class=\"errorText\">{0}</span>", error)));
        }

        private void ShowFBPublish()
        {
            if (CurrentPerson.PersonID != Constants.NULL_INT)
            {
                var registrant = (from r in registration.Registrants.OfType<Registrant>()
                                  where r.PersonID == CurrentPerson.PersonID
                                  select r).FirstOrDefault();

                if (registrant != null)
                {
                    StringBuilder fbScript = new StringBuilder();
                    fbScript.Append("\n\n<script language=\"javascript\">\n");
                    fbScript.AppendFormat("  callPublish('',{{'name':'{0}','href':'{1}','description':'{2}','media':[{{'type':'image','src':'image','href':'{3}'}}]}},null);", 
                        AntiXss.JavaScriptEncode("I just signed up for " + registration.EventProfile.Name, false),
                        BuildDetailPageUrl(registration.ProfileId), 
                        AntiXss.JavaScriptEncode(registration.EventProfile.Summary, false), 
                        BuildFullURL(registration.EventProfile.Image.ThumbnailUrl()));
                    fbScript.Append("</script>\n\n");
                    phFBPublish.Controls.Add(new LiteralControl(fbScript.ToString()));
                }
            }
        }

        private string BuildFullURL(string absolutePath)
        {
            string fullURL = Page.Request.Url.Scheme + "://" + Page.Request.Url.Host + (Page.Request.Url.IsDefaultPort ? "" : ":" + Page.Request.Url.Port) + Page.ResolveUrl(absolutePath);
            return fullURL;
        }

        private string BuildDetailPageUrl(int eventID)
        {
            string baseURL = Page.Request.Url.Scheme + "://" + Page.Request.Url.Host + (Page.Request.Url.IsDefaultPort ? "" : ":" + Page.Request.Url.Port) + Page.ResolveUrl("~/default.aspx");

            return string.Format("{0}?page={1}&eventid={2}", baseURL, EventDetailPageSetting, eventID);
        }
    }
}