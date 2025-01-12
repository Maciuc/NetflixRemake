namespace exp.Services.Generic
{
    public interface IGenericService
    {
        string? CreateImagePath(string? newImgBase64, string? oldImgPath, string folderName);
        string? CreateImageUrl(string? imagePath);
        string? UpdateImagePath(string? newImageBase64, string? oldImagePath, string className);
        bool ValidateEmail(string emailAddress);
        bool ValidatePhoneNumber(string phoneNumber);
        bool ValidatePostalCode(string postalCode);
        bool ValidateTime(string start, string end);
        bool ValidateDay(string day);
        int GetDayNumber(string day);
        bool ValidateDate(string start, string end);
        string GeneratePassword();
        string? StringToSlug(string? str);
        bool IsDateInCurrentWeek(DateTime? date);
        DateTime GetStartOfCurrentWeek();
        DateTime GetEndOfCurrentWeek();
        bool ArePropertiesEqual<T>(T obj1, T obj2);
        bool CheckIfWordsExist(string str1, string str2, string separator);
        string FormatPeriod(DateOnly? startDate, DateOnly? endDate);
        bool PeriodsIntersect(DateOnly startDate1, DateOnly endDate1, DateOnly startDate2, DateOnly endDate2);
    }
}