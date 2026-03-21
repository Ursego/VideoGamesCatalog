// A generic type for a dropdown list entry.

namespace VideoGameCatalog.Api.Util
{
    public class DropdownEntryDto
    {
        public int Id { get; set; }
        
        private string _description = null!;
        public string Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Description cannot be empty.", nameof(value));
                }

                _description = value;
            }
        }

        public DropdownEntryDto() {} // required for serialization

        public DropdownEntryDto(int id, string description)
        {
            Id = id;
            Description = description; // validated by the setter
        }
    }
}
