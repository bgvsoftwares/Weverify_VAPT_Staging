using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class clsValidateImage
    {
    }

    //Customized data annotation validator for uploading file
    public class ValidateImageFileAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                int MaxContentLength = 1024 * 1024 * 100; //1 MB
                string[] AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".gif", ".png", ".JPG", ".JPEG", ".GIF", ".PNG", ".ZIP", ".zip"};

                var file = value as HttpPostedFileBase;

                if (file == null)
                    return false;
                else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                {
                    ErrorMessage = "Please upload Image of type: " + string.Join(", ", AllowedFileExtensions);
                    return false;
                }
                else if (file.ContentLength > MaxContentLength)
                {
                    ErrorMessage = "Image is too large, maximum allowed size is : " + (MaxContentLength / 1024).ToString() + "KB";
                    return false;
                }
                else
                    return true;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
    }

    public class ValidateImageFileWithNullCheckAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                int MaxContentLength = 1024 * 1024 * 100; //1 MB
                string[] AllowedFileExtensions = new string[] { ".pdf", ".doc", ".docx",".jpg", ".jpeg",".gif", ".png", ".PDF", ".DOC", ".DOCX", ".JPG", ".JPEG", ".GIF", ".PNG", ".mp3", ".WAV", ".wav", ".M4A", ".m4a" };

                var file = value as HttpPostedFileBase;

                if (file == null)
                    return true;
                else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                {
                    ErrorMessage = "Please upload Image of type: " + string.Join(", ", AllowedFileExtensions);
                    return false;
                }
                else if (file.ContentLength > MaxContentLength)
                {
                    ErrorMessage = "Image is too large, maximum allowed size is : " + (MaxContentLength / 1024).ToString() + "KB";
                    return false;
                }
                else
                    return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class ValidateFileWithNullCheckAttributeInPartner : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                int MaxContentLength = 1024 * 1024 * 100; //1 MB
                string[] AllowedFileExtensions = new string[] { ".pdf", ".jpg", ".jpeg", ".png", ".PDF", ".JPG", ".JPEG", ".PNG" };

                var file = value as HttpPostedFileBase;

                if (file == null)
                    return true;
                else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                {
                    ErrorMessage = "Please upload Image of type: " + string.Join(", ", AllowedFileExtensions);
                    return false;
                }
                else if (file.ContentLength > MaxContentLength)
                {
                    ErrorMessage = "Image is too large, maximum allowed size is : " + (MaxContentLength / 1024).ToString() + "KB";
                    return false;
                }
                else
                    return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    //Customized data annotation validator for uploading file
    public class ValidateLargeFileAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                int MaxContentLength = 1024 * 1024 * 100; //30 MB
                //string[] AllowedFileExtensions = new string[] { ".pdf", ".doc", ".docx",".jpg", ".jpeg", ".gif", ".png" };
                string[] AllowedFileExtensions = new string[] { ".pdf", ".jpg", ".jpeg",".png", ".PDF", ".JPG", ".JPEG", ".PNG", ".Mp3", ".mp3", ".MP3", ".WAV", ".wav", ".M4A", ".m4a", ".MSG",".msg" , ".mp4", ".MP4" , ".Mp4" };
                var file = value as HttpPostedFileBase;

                if (file == null)
                    return false;
                else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                {
                    ErrorMessage = "Please upload File of type: " + string.Join(", ", AllowedFileExtensions);
                    return false;
                }
                else if (file.ContentLength > MaxContentLength)
                {
                    ErrorMessage = "File is too large, maximum allowed size is : " + (MaxContentLength / 1024).ToString() + "KB";
                    return false;
                }
                else
                    return true;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
    }

    //Customized data annotation validator for uploading file
    public class ValidateImageFileupload : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                int MaxContentLength = 1024 * 1024 * 100; //1 MB
                //string[] AllowedFileExtensions = new string[] { ".pdf", ".doc", ".docx",".jpg", ".jpeg", ".gif", ".png" };
                string[] AllowedFileExtensions = new string[] {  ".jpg", ".jpeg",  ".JPG", ".JPEG" };
                var file = value as HttpPostedFileBase;

                if (file == null)
                    return false;
                else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                {
                    ErrorMessage = "Please upload File of type: " + string.Join(", ", AllowedFileExtensions);
                    return false;
                }
                else if (file.ContentLength > MaxContentLength)
                {
                    ErrorMessage = "File is too large, maximum allowed size is : " + (MaxContentLength / 1024).ToString() + "KB";
                    return false;
                }
                else
                    return true;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
    }

    //Customized data annotation validator for uploading file
    public class ValidateLargeFileCandidateUploadDocs : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                int MaxContentLength = 1024 * 1024 * 100; //30 MB
                string[] AllowedFileExtensions = new string[] {".doc", ".docx", ".xls", ".xlsx", ".pdf",".rtf", ".txt", ".jpeg",".jpg", ".png", ".eml", ".email",".msg",".zip",".rar", ".DOC", ".DOCX", ".XLS", ".XLSX", ".PDF", ".RTF", ".TXT", ".JPEG", ".JPG", ".PNG", ".EML", ".EMAIL", ".msg", ".MSG", ".ZIP", ".RAR", ".ZIPX", ".zipx", ".MP3", ".mp3", ".WAV", ".wav", ".M4A", ".m4a", ".webp", ".WEBP" };

                var file = value as HttpPostedFileBase;

                if (file == null)
                    return true;
                else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                {
                    ErrorMessage = "Please upload File of type: " + string.Join(", ", AllowedFileExtensions);
                    return false;
                }
                else if (file.ContentLength > MaxContentLength)
                {
                    ErrorMessage = "File is too large, maximum allowed size is : " + (MaxContentLength / 1024).ToString() + "KB";
                    return false;
                }
                else
                    return true;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
    }

    public class ValidateChildFileUploadedByClient : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                int MaxContentLength = 1024 * 1024 * 100; //30 MB
                string[] AllowedFileExtensions = new string[] { ".doc", ".docx", ".xls", ".xlsx", ".pdf", ".rtf", ".txt", ".jpeg", ".jpg", ".png", ".DOC", ".DOCX", ".XLS", ".XLSX", ".PDF", ".RTF", ".TXT", ".JPEG", ".JPG", ".PNG", ".MP3", ".mp3", ".WAV", ".wav", ".M4A", ".m4a", ".MSG" };
                var file = value as HttpPostedFileBase;
                if (file == null)
                    return true;               
                else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                {
                    ErrorMessage = "Only,.doc,.xls,.pdf,Image and Zip File allowed";
                    return false;
                }
                else if (file.ContentLength > MaxContentLength)
                {
                    ErrorMessage = "File is too large, maximum allowed size is : " + (MaxContentLength / 1024).ToString() + "KB";
                    return false;
                }
                else
                    return true;
                }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
    }

    public class ValidateParentFileUploadedByClient : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                int MaxContentLength = 1024 * 1024 * 100; //30 MB
                string[] AllowedFileExtensions = new string[] { ".doc", ".docx", ".xls", ".xlsx", ".pdf", ".rtf", ".txt", ".jpeg", ".jpg", ".png", ".DOC", ".DOCX", ".XLS", ".XLSX", ".PDF", ".RTF", ".TXT", ".JPEG", ".JPG", ".PNG", ".ZIP", ".zip", ".MP3", ".mp3", ".WAV", ".wav", ".M4A", ".m4a", ".MSG" };
                string[] AllowedFileExtensions1 = new string[] { ".doc", ".xls", ".pdf", ".jpeg", ".email", ".msg", ".PNG", ".MP3"};

                var file = value as HttpPostedFileBase;
                if (file == null)
                    return true;
                else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                {
                    ErrorMessage = "Only These files are Allowd in ZIP File : " + string.Join(", ", AllowedFileExtensions1);
                    return false;
                }
                else if (file.ContentLength > MaxContentLength)
                {
                    ErrorMessage = "File is too large, maximum allowed size is : " + (MaxContentLength / 1024).ToString() + "KB";
                    return false;
                }
                else
                    return true;
            }

            catch (Exception Ex)
            {
                throw Ex;
            }
        }
    }


    public class ValidateChildFileUploadedByCandidate : ValidationAttribute 
    {
        public override bool IsValid(object value)
        {
            try
            {
                int MaxContentLength = 2048 * 2048 * 200; //30 MB               
                string[] AllowedFileExtensions = new string[] { ".doc", ".docx", ".xls", ".xlsx", ".pdf", ".rtf", ".txt", ".jpeg", ".jpg", ".png", ".DOC", ".DOCX", ".XLS", ".XLSX", ".PDF", ".RTF", ".TXT", ".JPEG", ".JPG", ".PNG",".MP3", ".mp3", ".WAV", ".wav", ".M4A", ".m4a", ".msg", ".MSG" };
                var file = value as HttpPostedFileBase;
                if (file == null)
                    return true;
                else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                {
                    ErrorMessage = "Only,.doc,.xls,.pdf,Image and Zip File allowed";
                    return false;
                }
                //else if (file.ContentLength > MaxContentLength)
                //{
                //    ErrorMessage = "File is too large, maximum allowed size is : " + (MaxContentLength / 1024).ToString() + "KB";
                //    return false;
                //}
                else
                    return true;               
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
    }
    public class ValidateParentFileUploadedByCandidate : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                int MaxContentLength = 1024 * 1024 * 100; //30 MB
                string[] AllowedFileExtensions = new string[] { ".doc", ".docx", ".xls", ".xlsx", ".pdf", ".rtf", ".txt", ".jpeg", ".jpg", ".png", ".DOC", ".DOCX", ".XLS", ".XLSX", ".PDF", ".RTF", ".TXT", ".JPEG", ".JPG", ".PNG" , ".ZIP", ".zip",".MP3" , ".mp3", ".WAV", ".wav", ".M4A", ".m4a", ".msg", ".MSG" };
                string[] AllowedFileExtensions1=new string[] { ".doc", ".xls", ".pdf",  ".jpeg", ".email", ".msg",".PNG", ".MP3" };
              
                var file = value as HttpPostedFileBase;
                if (file == null)
                    return true;
                else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                {
                    ErrorMessage = "Only These files are Allowd in ZIP File : " + string.Join(", ", AllowedFileExtensions1);
                    return false;
                }
                else if (file.ContentLength > MaxContentLength)
                {
                    ErrorMessage = "File is too large, maximum allowed size is : " + (MaxContentLength / 1024).ToString() + "KB";
                    return false;
                }
                else
                    return true;
            }
            
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
    }

    public class ValidateChildFileUploadedByTeam : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                int MaxContentLength = 1024 * 1024 * 100; //30 MB                 
                string[] AllowedFileExtensions = new string[] { ".doc", ".docx", ".xls", ".xlsx", ".pdf", ".rtf", ".txt", ".jpeg", ".jpg", ".png", ".eml", ".email", ".msg", ".DOC", ".DOCX", ".XLS", ".XLSX", ".PDF", ".RTF", ".TXT", ".JPEG", ".JPG", ".PNG", ".EML", ".EMAIL", ".MSG" , ".MP3", ".mp3", ".WAV", ".wav", ".M4A", ".m4a" };
                var file = value as HttpPostedFileBase;
                if (file == null)
                    return true;
                else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                {
                    ErrorMessage = "Only,.doc,.xls,.pdf,.MP3 Image and Zip File allowed";
                    return false;
                }
                else if (file.ContentLength > MaxContentLength)
                {
                    ErrorMessage = "File is too large, maximum allowed size is : " + (MaxContentLength / 1024).ToString() + "KB";
                    return false;
                }
                else
                    return true;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
    }
    public class ValidateParentFileUploadedByTeam : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                int MaxContentLength = 1024 * 1024 * 100; //30 MB
                string[] AllowedFileExtensions = new string[] { ".doc", ".docx", ".xls", ".xlsx", ".pdf", ".rtf", ".txt", ".jpeg", ".jpg", ".png", ".eml", ".email", ".msg", ".DOC", ".DOCX", ".XLS", ".XLSX", ".PDF", ".RTF", ".TXT", ".JPEG", ".JPG", ".PNG", ".EML", ".EMAIL", ".MSG",".ZIP", ".zip", ".MP3", ".mp3", ".WAV", ".wav", ".M4A", ".m4a" };
                string[] AllowedFileExtensions1 = new string[] { ".doc", ".xls", ".pdf", ".jpeg", ".email", ".msg", ".PNG", ".MP3" };

                var file = value as HttpPostedFileBase;
                if (file == null)
                    return true;
                else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                {
                    ErrorMessage = "Only These files are Allowd in ZIP File : " + string.Join(", ", AllowedFileExtensions1);
                    return false;
                }
                else if (file.ContentLength > MaxContentLength)
                {
                    ErrorMessage = "File is too large, maximum allowed size is : " + (MaxContentLength / 1024).ToString() + "KB";
                    return false;
                }
                else
                    return true;
            }

            catch (Exception Ex)
            {
                throw Ex;
            }
        }
    }

    //Customized data annotation validator for uploading file
    public class ValidateLargeFileCandidateUploadExcelDocs : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                int MaxContentLength = 1024 * 1024 * 100; //30 MB
                string[] AllowedFileExtensions = new string[] {".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".gif", ".png", ".zip", ".rar", ".PDF", ".DOC", ".DOCX", ".JPG", ".JPEG", ".GIF", ".PNG", ".ZIP", ".RAR", ".MP3", ".mp3", ".WAV", ".wav", ".M4A", ".m4a" };

                var file = value as HttpPostedFileBase;

                if (file == null)
                    return true;
                else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                {
                    ErrorMessage = "Please upload File of type: " + string.Join(", ", AllowedFileExtensions);
                    return false;
                }
                else if (file.ContentLength > MaxContentLength)
                {
                    ErrorMessage = "File is too large, maximum allowed size is : " + (MaxContentLength / 1024).ToString() + "KB";
                    return false;
                }
                else
                    return true;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
    }

    public class ValidateLargeFileClientCandidateUploadExcelDocs : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                int MaxContentLength = 1024 * 1024 * 100; //1MB
                string[] AllowedFileExtensions = new string[] { ".xls", ".xlsx"};

                var file = value as HttpPostedFileBase;

                if (file == null)
                    return true;
                else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                {
                    ErrorMessage = "Please upload File of type: " + string.Join(", ", AllowedFileExtensions);
                    return false;
                }
                else if (file.ContentLength > MaxContentLength)
                {
                    ErrorMessage = "File is too large, maximum allowed size is : " + (MaxContentLength / 1024).ToString() + "KB";
                    return false;
                }
                else
                    return true;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
    }
}