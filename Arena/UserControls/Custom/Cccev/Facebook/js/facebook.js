/**********************************************************************
* Description:  Scripts for common Facebook functionality
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 2/16/2010
*
* $Workfile: facebook.js $
* $Revision: 2 $
* $Header: /trunk/Arena/UserControls/Custom/Cccev/Facebook/js/facebook.js   2   2010-02-17 09:53:02-07:00   JasonO $
*
* $Log: /trunk/Arena/UserControls/Custom/Cccev/Facebook/js/facebook.js $
*  
*  Revision: 2   Date: 2010-02-17 16:53:02Z   User: JasonO 
*  Fixing jquery include issues. 
*  
*  Revision: 1   Date: 2010-02-17 00:52:05Z   User: JasonO 
**********************************************************************/

/// <Summary>
/// Wrapper class to abstract common Facebook functionality
/// </Summary>
function Cccev_Facebook()
{
    this.init = function(key, path)
    {
        if (!fbInited)
        {
            FB.init(key, path);
            fbInited = true;
        }
    };

    this.publish = function(msg, attachment, action_link)
    {
        FB.ensureInit(function()
        {
            FB.Connect.streamPublish(msg, attachment, action_link, null, 'Share this with your friends.');
        });
    };
}

/// <Summary>
/// Scripts to initialize and execute custom Facebook code
/// </Summary>
var fbInited = false;
var fb = null;

function initFacebook(key, receiverPath)
{
    $('html').attr('xmlns:fb', 'http://www.facebook.com/2008/fbml');
    fb = new Cccev_Facebook();
    fb.init(key, receiverPath);
}

function callPublish(msg, attachment, action_link)
{
    fb.publish(msg, attachment, action_link);
}