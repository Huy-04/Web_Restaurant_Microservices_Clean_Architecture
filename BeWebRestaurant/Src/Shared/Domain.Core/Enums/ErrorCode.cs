namespace Domain.Core.Enums
{
    public enum ErrorCode
    {
        IdNotFound,

        InUse,

        // Relationship mismatch
        NoMatchingCombination,

        InvalidStatus,

        NameEmpty,
        NameTooLong,
        NameAlreadyExists,

        NotZero,
        NotNegative,
        ExceedsMaximum,

        // Duplicate Property
        DuplicateEntry,

        //Collection
        CollectionEmpty,

        TypeMismatch,
    }
}