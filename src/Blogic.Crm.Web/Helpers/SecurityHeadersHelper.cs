namespace Blogic.Crm.Web.Helpers;

/// <summary>
///      Helper functions for managing application's security header.
/// </summary>
public static class SecurityHeadersHelper
{
    /// <summary>
    ///     Adds security headers to the application.
    /// </summary>
    public static void AddSecurityHeaders(this IApplicationBuilder application)
    {
        var policyCollection = new HeaderPolicyCollection()
            .AddFrameOptionsDeny()
            .AddXssProtectionBlock()
            .AddContentTypeOptionsNoSniff()
            .AddStrictTransportSecurityMaxAgeIncludeSubDomains() // 1 Year
            .AddReferrerPolicyStrictOriginWhenCrossOrigin()
            .RemoveServerHeader()
            .AddContentSecurityPolicy(builder =>
            {
                builder.AddObjectSrc().None();
                builder.AddFormAction().Self();
                builder.AddFrameAncestors().None();
                builder.AddStyleSrc().Self();
                builder.AddScriptSrc().Self();
                builder.AddFontSrc().Self();
                builder.AddMediaSrc().Self();
                builder.AddConnectSrc().Self();
            })
            .AddCrossOriginOpenerPolicy(builder => { builder.SameOrigin(); })
            .AddCrossOriginEmbedderPolicy(builder => { builder.RequireCorp(); })
            .AddCrossOriginResourcePolicy(builder => { builder.SameOrigin(); })
            .AddPermissionsPolicy(builder =>
            {
                builder.AddAmbientLightSensor().None();
                builder.AddAccelerometer().None();
                builder.AddAutoplay().Self();
                builder.AddCamera().None();
                builder.AddEncryptedMedia().Self();
                builder.AddFullscreen().All();
                builder.AddGeolocation().None();
                builder.AddGyroscope().None();
                builder.AddMagnetometer() .None();
                builder.AddMicrophone() .None();
                builder.AddMidi().None();
                builder.AddPayment().None();
                builder.AddPictureInPicture().None();
                builder.AddSpeaker().None();
                builder.AddSyncXHR().None();
                builder.AddUsb().None();
                builder.AddVR().None();
            });

        application.UseSecurityHeaders(policyCollection);
    }
}