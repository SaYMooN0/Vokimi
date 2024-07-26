using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.test_creation.generic_test_related;
using VokimiShared.src.models.db_classes.tests;

namespace VokimiShared.src.models.db_classes.test.test_types
{
    public class TestGenericType : BaseTest
    {
        public override TestTemplate Template => TestTemplate.Generic;

        public virtual ICollection<GenericTestQuestion> Questions { get; init; } = [];
       //results  public virtual ICollection<> PossibleResults { get; init; } =[];

    
    }
}
