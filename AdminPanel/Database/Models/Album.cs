using System;
using System.Collections.Generic;

namespace AdminPanel.Database.Models
{
    [GeneratedController("admin/Album")]
    public class Album
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public List<Book> Books { get; set; }
    }
}
