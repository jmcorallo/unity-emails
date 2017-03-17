using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendEmailExample : MonoBehaviour
{
    public InputField SenderEmail;
    public InputField SenderPassword;
    public InputField SmtpClient;
    public InputField SmtpPort;
    public InputField ToEmail;
    public InputField Subject;
    public InputField Body;
    public InputField[] Attachments;
    public Toggle IsHtml;

    public void SendEmail()
    {
        EmailSender.SenderEmail = SenderEmail.text;
        EmailSender.SenderPassword = SenderPassword.text;
        EmailSender.SmtpClient = SmtpClient.text;
        // TODO check if port is valid
        EmailSender.SmtpPort = int.Parse(SmtpPort.text);

        List<string> atts = new List<string>();
        foreach (InputField field in Attachments)
        {
            if (!string.IsNullOrEmpty(field.text))
            {
                atts.Add(field.text);
            }
        }
        EmailSender.SendEmail(ToEmail.text, Subject.text, Body.text, IsHtml.isOn, atts.ToArray());
    }

    public void AddSampleAttachment(int index)
    {
        Attachments[index].text = System.IO.Path.Combine(Application.streamingAssetsPath, string.Format("SampleAttachment{0}.png", index));
    }
}