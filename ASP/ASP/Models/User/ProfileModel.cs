namespace ASP.Models.User
{
    public class ProfileModel
    {
        public Guid Id { get; set; }
        public String Login { get; set; }
        public String RealName { get; set; }
        public String Email { get; set; }
        public String? Avatar { get; set; }
        public DateTime RegisterDt { get; set; }
        public DateTime? LastEnterDt { get; set; }
        public Boolean IsEmailPublic { get; set; }
        public Boolean IsRealNamePublic { get; set; }
        public Boolean IsDatesPublic { get; set; }

        public ProfileModel()
        {
            Login = RealName = Email = null!;
        }

        public Boolean IsPersonal { get; set; } = false;


        public ProfileModel(Data.Entity.User user)
        {
            var userProps = user.GetType().GetProperties();
            var thisProps = this.GetType().GetProperties();
            foreach (var thisProp in thisProps)
            {
                var prop = userProps.FirstOrDefault(userProp =>
                    userProp.Name == thisProp.Name
                    &&
                    userProp.PropertyType.IsAssignableTo(thisProp.PropertyType)
                );
                if (prop is not null)
                {
                    thisProp.SetValue(this, prop.GetValue(user));
                }
            }
        }


    }
}
