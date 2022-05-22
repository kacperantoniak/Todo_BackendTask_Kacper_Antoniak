using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ToDo.Models
{
    [Table("ToDos")]
    public record TodoModel
    {
        //I've decided to validate data through json properties since it's the easiest way known to me
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        //this one must have a specific function to assign value, that's why JsonIgnore property is here
        [Range(0,100)]
        [JsonPropertyName("%OfCompletion")]
        public int Completion { get; set; }
        [MaxLength(255)]
        [Required]
        public string Title { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        //this one must have a specific function to assign value, that's why JsonIgnore property is here
        [JsonPropertyName("IsCompleted?")]
        public bool Complete { get; set; }
        public DateTime CreationDate { get; set; }
        [JsonPropertyName("ExpiryDate")]
        public DateTime Expires { get; set; }
    }
}
