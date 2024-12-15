using Pokemon_AP_Project_G3.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon_AP_Project_G3.Data.Repository
{
    public class MessageRepository
    {
        private readonly PokemonGameEntities _context;

        public MessageRepository(PokemonGameEntities context)
        {
            _context = context;
        }

        public List<Messages> GetRecentMessages(int count = 20)
        {
            return _context.Messages
                           .OrderByDescending(m => m.sent_date)
                           .Take(count)
                           .ToList();
        }

        public void AddMessage(string senderUsername, string receiverUsername, string content)
        {
            senderUsername = senderUsername?.Trim();
            receiverUsername = receiverUsername?.Trim();

            var sender = _context.Users.FirstOrDefault(u => u.username == senderUsername);
            var receiver = _context.Users.FirstOrDefault(u => u.username == receiverUsername);

            if (sender != null && receiver != null)
            {
                var message = new Messages
                {
                    sender_id = sender.user_id,
                    receiver_id = receiver.user_id,
                    content = content,
                    sent_date = DateTime.Now
                };

                _context.Messages.Add(message);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception($"No se encontraron los usuarios especificados: {senderUsername}, {receiverUsername}");
            }
        }


    }
}
