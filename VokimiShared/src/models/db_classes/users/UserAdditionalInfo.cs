using VokimiShared.src.models.db_entities_ids;

namespace VokimiShared.src.models.db_classes.users
{
    public class UserAdditionalInfo
    {
        public UserAdditionalInfoId Id { get; private set; }
        public DateTime RegistrationDate { get; init; }
        public DateTime? BirthDate { get; init; }
        public static UserAdditionalInfo CreateNew(DateTime registrationDate) =>
            new()
            {
                Id = new UserAdditionalInfoId(),
                RegistrationDate = registrationDate,
                BirthDate = null
            };
    }
}
