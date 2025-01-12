using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace exp.Services.Generic
{
    public class GenericService : IGenericService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GenericService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        private (string, string) ExtrageImagBase64(string dataUri)
        {
            string delim = ";base64,";

            if (!dataUri.Contains(delim))
            {
                throw new InvalidDataException("Image format is not valid");
            }

            string[] parts = dataUri.Split(delim, 2, StringSplitOptions.None);
            string[] parts2 = parts[0].Split('/');

            string imageExtension = parts2[1];
            string imageBase64 = parts[1];

            //if (imageExtension.Contains("svg+xml"))
            //{
            //    imageExtension = "svg";
            //}

            return (imageExtension, imageBase64);
        }

        private string Base64Type(string data)
        {
            string type;
            switch (data.ToUpper())
            {
                case "IVBOR":
                    type = "png";
                    break;
                case "/9J/4":
                    type = "jpg";
                    break;
                default:
                    type = "jpg";
                    break;
            }
            string format = "data:image/" + type + ";base64,";
            return format;
        }

        public string? CreateImagePath(string? newImgBase64, string? oldImgPath, string folderName)
        {
            string? imagesFolder = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("PathForImages")["path"];
            var currentDirectory = Directory.GetCurrentDirectory();
            string baseFolderPath = currentDirectory + "\\" + imagesFolder;

            string? oldImageFilePath = File.Exists(baseFolderPath + oldImgPath) ? baseFolderPath + oldImgPath : null;

            if (String.IsNullOrWhiteSpace(newImgBase64))//If user doesn t select an image, we delete the old one from backend
            {
                if (!String.IsNullOrWhiteSpace(oldImageFilePath))
                {
                    File.Delete(oldImageFilePath);
                }
                return null;
            }

            if (!String.IsNullOrWhiteSpace(oldImageFilePath))//If user update the image, delete the old one, otherwise keep the old path
            {
                var oldImageBase64 = Convert.ToBase64String(File.ReadAllBytes(oldImageFilePath));
                oldImageBase64 = Base64Type(oldImageBase64.Substring(0, 5)) + oldImageBase64;

                if (oldImageBase64 != newImgBase64)
                {
                    File.Delete(oldImageFilePath);
                }
                else
                {
                    return oldImgPath;
                }
            }

            string folderPath = baseFolderPath + "\\" + folderName;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var (imgType, ImageBase64) = ExtrageImagBase64(newImgBase64);

            byte[] imageBytes = Convert.FromBase64String(ImageBase64);
            string randomFileName = Path.GetRandomFileName() + "." + imgType;

            string imagePathToWrite = folderPath + "\\" + randomFileName;
            File.WriteAllBytes(imagePathToWrite, imageBytes);

            var newImagePath = "\\" + folderName + "\\" + randomFileName;

            return newImagePath;
        }

        public string? CreateImageUrl(string? imagePath)
        {
            if (imagePath == null)
            {
                return null;
            }

            string? prodEnv = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ProdEnvironment").Value;

            var request = _httpContextAccessor.HttpContext.Request;
            imagePath = imagePath.Replace("\\", "/");

            string createdUrl = request.Scheme + "://" + request.Host + prodEnv + imagePath;

            return createdUrl;
        }

        public string? UpdateImagePath(string? newImageBase64, string? oldImagePath, string className)
        {
            if (newImageBase64 == null)
            {
                return null;
            }

            //Verify if we got an Url from frontend(that means the image is unchanged, so we return the image path from url)
            var request = _httpContextAccessor.HttpContext.Request.Scheme;
            if (newImageBase64.StartsWith(request))
            {
                return $"/{className}/" + newImageBase64.Split(className)[1];
            }

            return CreateImagePath(newImageBase64, oldImagePath, className);
        }

        public bool ValidateEmail(string emailAddress)
        {
            // regex pt validare email
            // https://github.com/jquense/yup/issues/507
            // / ^(([^<> ()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)| (".+"))@((\[[0 - 9]{ 1,3}\.[0 - 9]{ 1,3}\.[0 - 9]{ 1,3}\.[0 - 9]{ 1,3}])| (([a - zA - Z\-0 - 9] +\.)+[a - zA - Z]{ 2,}))$/
            try
            {
                var email = new MailAddress(emailAddress);
                return email.Address == emailAddress.Trim();
            }
            catch
            {
                return false;
            }
        }

        public bool ValidatePhoneNumber(string number)
        {
            string motif = @"^([\+]?40[-]?|[0])?[1-9][0-9]{8}$";

            if (number != null)
            {
                return Regex.IsMatch(number, motif);
            }
            else
            {
                return false;
            }
        }

        public bool ValidatePostalCode(string postalCode)
        {
            string motif = @"^[0-9]{6}$";

            if (postalCode != null)
            {
                return Regex.IsMatch(postalCode, motif);
            }
            else
            {
                return false;
            }
        }

        public bool ValidateDay(string day)
        {
            var daysOfTheWeek = new List<string>()
        {
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday",
        };

            if (daysOfTheWeek.Contains(day))
            {
                return true;
            }

            return false;
        }

        public int GetDayNumber(string day)
        {
            var daysOfTheWeek = new List<string>()
        {
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday",
        };

            return daysOfTheWeek.IndexOf(day);
        }

        public bool ValidateTimeFormat(string time)
        {
            string motif = @"^([0-1][0-9]|2[0-3]):[0-5][0-9]$";
            if (time != null)
            {
                return Regex.IsMatch(time, motif);
            }
            else
            {
                return false;
            }
        }

        public bool ValidateDate(string start, string end)
        {
            try
            {
                DateTime startDate = DateTime.Parse(start);
                DateTime endDate = DateTime.Parse(end);

                if (startDate > endDate)
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool ValidateTime(string start, string end)
        {
            if (ValidateTimeFormat(start) && ValidateTimeFormat(end))
            {
                if (DateTime.Parse(start) < DateTime.Parse(end))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public string GeneratePassword()
        {
            const int passwordLength = 35;
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890qwzwaesxrdctfvygbuhnertyuiopasdfghjklzxcvbnm!@#$%^&*(){}|[]',./<>? ";

            StringBuilder password = new StringBuilder();
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] randomBytes = new byte[passwordLength];

                rng.GetBytes(randomBytes);

                for (int i = 0; i < passwordLength; i++)
                {
                    int randomIndex = randomBytes[i] % validChars.Length;
                    password.Append(validChars[randomIndex]);
                }
            }

            return password.ToString();
        }

        public string? StringToSlug(string? str)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            str = str.ToLower();
            Dictionary<char, char> diacriticsMap = new Dictionary<char, char>()
        {
            {'ă', 'a'},
            {'â', 'a'},
            {'î', 'i'},
            {'ș', 's'},
            {'ț', 't'}
        };

            foreach (var pair in diacriticsMap)
            {
                str = str.Replace(pair.Key, pair.Value);
            }

            str = Regex.Replace(str, @"[^a-z0-9\s-]", ""); // Elimină caracterele non-alfanumerice și non-'-' sau spații
            str = Regex.Replace(str, @"\s+", "-"); // Înlocuiește spațiile consecutive cu un singur '-'
            str = str.Trim(); // Elimină spațiile de la început și sfârșit

            return str;
        }

        public bool IsDateInCurrentWeek(DateTime? date)
        {
            if (date == null)
                return false;

            DateTime now = DateTime.Now;
            Calendar calendar = CultureInfo.CurrentCulture.Calendar;
            DateTime firstDayOfWeek = now.Date.AddDays(((int)calendar.GetDayOfWeek(now) - (int)DayOfWeek.Monday) * (-1));
            DateTime lastDayOfWeek = firstDayOfWeek.AddDays(6);

            return date.Value.Date >= firstDayOfWeek && date.Value.Date <= lastDayOfWeek;
        }

        public DateTime GetStartOfCurrentWeek()
        {
            var today = DateTime.Today;
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            return today.AddDays(-1 * diff).Date;
        }

        public DateTime GetEndOfCurrentWeek()
        {
            return GetStartOfCurrentWeek().AddDays(7).AddTicks(-1);
        }

        public bool ArePropertiesEqual<T>(T obj1, T obj2)
        {
            if (obj1 == null || obj2 == null)
                return false;

            foreach (var property in typeof(T).GetProperties())
            {
                if (property.PropertyType.IsEnum || property.PropertyType.IsCollectible)
                {
                    continue;
                }

                var value1 = property.GetValue(obj1);
                var value2 = property.GetValue(obj2);

                if (!Equals(value1, value2))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check if words from first string exist in second string
        /// </summary>
        public bool CheckIfWordsExist(string str1, string str2, string separator)
        {
            var words1 = str1.Split(separator).Select(word => word.Trim());
            var words2 = str2.Split(separator).Select(word => word.Trim());

            return words1.All(word => words2.Contains(word));
        }

        public string FormatPeriod(DateOnly? startDate, DateOnly? endDate)
        {
            //return (startDate.ToString() + " - " + endDate.ToString()).Replace("/", ".");
            return $"{startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}";
        }

        public bool PeriodsIntersect(DateOnly startDate1, DateOnly endDate1, DateOnly startDate2, DateOnly endDate2)
        {
            return startDate1 <= endDate2 && endDate1 >= startDate2;
        }
    }
}


