namespace HIDAeroService.Dto.AccessLevel
{
    public sealed class CreateAccessLevelDto
    {
        public string Name { get; set; }
        public short AccessLevelNumner { get; set; }
        public short Mode { get; set; }
        public List<CreateAccessLevelDoor> Doors { get; set; }
    }
}
