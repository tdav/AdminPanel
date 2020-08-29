using System;

namespace AdminPanel.Database.Models
{
    [GeneratedController("admin/Book")]
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public int AlbumId { get; set; }
        public Album Album { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
