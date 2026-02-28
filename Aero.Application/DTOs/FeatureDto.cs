

namespace Aero.Application.DTOs
{


    public sealed record FeatureDto(int Id,string Name,string Path,List<SubFeatureDto> SubItem,bool IsAllow,bool IsCreate,bool IsModify,bool IsDelete,bool IsAction);
}
