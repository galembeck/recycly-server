using Domain.Utils;

namespace Domain.Enumerators;

public enum ProfileType
{
    [EnumDescription("ADMIN")]
    ADMIN = 1,

    [EnumDescription("CLIENT")]
    CLIENT = 2,
}
