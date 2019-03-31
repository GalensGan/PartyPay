namespace PayPartyMemberDues
{
    public class APartyMemberInfo
    {
        private string _name = "李四";
        private string _gender = "男";
        private string _id = "123456789987654321";
        private string _appPhoneNumber = "12345678901";
        private double _duesPerMonth = 10;
        private double _monthCount = 3;
        private string _date = "1月到三月";
        private string _department = "设计所";

        /// <summary>
        /// 欢迎语
        /// </summary>
        public string WelcomeString => "尊敬的党员【" + _name + "】同志，欢迎使用党费自助缴纳系统！";


        /// <summary>
        /// 所需缴纳的钱
        /// </summary>
        public double TotalMoney => _duesPerMonth * _monthCount;

        public string Name { get => _name; set => _name = value; }
        public string Gender { get => _gender; set => _gender = value; }
        public string Id { get => _id; set => _id = value; }
        public string AppPhoneNumber { get => _appPhoneNumber; set => _appPhoneNumber = value; }
        public double DuesPerMonth { get => _duesPerMonth; set => _duesPerMonth = value; }
        public double MonthCount { get => _monthCount; set => _monthCount = value; }
        public string Date { get => _date; set => _date = value; }
        public string Department { get => _department; set => _department = value; }
    }

}
