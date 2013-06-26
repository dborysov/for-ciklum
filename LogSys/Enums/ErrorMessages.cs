namespace LogSys.Enums
{
    public static class ErrorMessages
    {
        public const string NotYourProject = "You can't work with this project. Please, chose another one";
        public const string NotAllSelectedProjectsBelongToYou = "Not all selected projects belong to you";
        public const string WrongDateSelected = "You have selected the wrong date.";
        public const string TriedToLogMoreThen24HoursPerDay = "You can't log more then 24 hours per day";
        public const string InputDataWasIncorrect = "Input data was incorrect";
        public const string CheckedNothing = "You haven't chosen anything";
        public const string TriedDeleteLogsFromMultipleProjects = "You can't delete logs from multiple projects";
        public const string DublicatedReports = "You already have a report with this name";
        public const string DublicatedProjects = "You already have a project with this name";
        public const string ProjectNotFound = "Project was not found";
        public const string ProjectNameWasNotPassedToServer = "Project name was not passed to server";
        public const string ReportDoesNotExistOrWasDeleted = "The report does not exist or it was deleted";
    }
}