using Domain.Data.Entities._Base;
using Domain.Data.Entities._Base.Extension;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Data.Entities;

[Table("TBResponsible")]
public class Responsible : BaseEntity, IBaseEntity<Responsible>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
    public List<string> Phones { get; set; } = new();
    public DateTimeOffset? LastAccessAt { get; set; }
    public string? PasswordChangeToken { get; set; }
    public DateTimeOffset? PasswordChangeTokenExpiresAt { get; set; }



    #region .: METHODS :.

    public Responsible WithoutRelations(Responsible entity)
    {
        if (entity == null)
            return null!;

        var newEntity = new Responsible
        {
            Name = entity.Name,
            Email = entity.Email,
            Cpf = entity.Cpf,
            Password = entity.Password,
            BirthDate = entity.BirthDate,
            Phones = entity.Phones,
            LastAccessAt = entity.LastAccessAt,
            PasswordChangeToken = entity.PasswordChangeToken,
            PasswordChangeTokenExpiresAt = entity.PasswordChangeTokenExpiresAt,
        };

        newEntity.InitializeInstance(entity);

        return newEntity;
    }

    #endregion .: METHODS :.
}
