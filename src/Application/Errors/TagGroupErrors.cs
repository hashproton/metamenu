namespace Application.Errors;

public static class TagGroupErrors
{
    public static readonly Error TagGroupAlreadyExists = Error.Conflict("tag_group_already_exists", "Tag group with the same name for the tenant already exists");
    
    public static readonly Error TagGroupNotFound = Error.NotFound("tag_group_not_found", "Tag group not found");
}