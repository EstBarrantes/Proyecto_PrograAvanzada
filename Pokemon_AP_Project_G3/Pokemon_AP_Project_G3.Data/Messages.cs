//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Pokemon_AP_Project_G3.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Messages
    {
        public int message_id { get; set; }
        public Nullable<int> sender_id { get; set; }
        public Nullable<int> receiver_id { get; set; }
        public string content { get; set; }
        public Nullable<System.DateTime> sent_date { get; set; }
    
        public virtual Users Users { get; set; }
        public virtual Users Users1 { get; set; }
    }
}