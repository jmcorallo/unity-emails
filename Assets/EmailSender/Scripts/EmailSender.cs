using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class EmailSender
{

    public static string SenderEmail;
    public static string SenderPassword;
    public static string SmtpClient;
    public static int SmtpPort;

    public static void SendEmail(string to, string subject, string body, bool isHtml, string[] attachmentPaths,
        Action<object, AsyncCompletedEventArgs> callback = null)
    {
        try
        {
            SmtpClient emailServer = new SmtpClient(SmtpClient, SmtpPort);
            emailServer.EnableSsl = true;
            emailServer.Credentials = (ICredentialsByHost) new NetworkCredential(SenderEmail, SenderPassword);

            // TODO this is unsafe, we're bypassing SSL certification check
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            MailMessage message = new MailMessage(SenderEmail, to);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = isHtml;

            foreach (string path in attachmentPaths)
            {
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    message.Attachments.Add(new Attachment(path));
                }
            }
            if (callback == null)
            {
                callback = SampleCallback;
            }
            emailServer.SendCompleted += new SendCompletedEventHandler(callback);
            emailServer.SendAsync(message, "");

            Debug.Log("Email sent async...");
        }
        catch (Exception ex)
        {
            Debug.Log("Error: " + ex.Message);
            callback("", new AsyncCompletedEventArgs(ex, true, "Exception occured"));
        }
    }

    private static void SampleCallback(object sender, AsyncCompletedEventArgs e)
    {
        if (e.Cancelled || e.Error != null)
        {
            Debug.Log("Error: " + e.Error.Message);
        }
        else
        {
            Debug.Log("Email sent successfully.");
        }
    }
}