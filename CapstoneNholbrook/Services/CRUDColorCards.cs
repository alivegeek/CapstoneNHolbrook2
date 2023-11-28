using CapstoneNHolbrook.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneNHolbrook.Services
{
    public class CRUDColorCards
    {
        private readonly DbOperations _dbOperations;

        public CRUDColorCards(DbOperations dbOperations)
        {
            _dbOperations = dbOperations;
        }

        public async Task AddColorCardAsync(ColorCard newColorCard)
        {
            try
            {
                Debug.WriteLine($"Adding ColorCard: ColorMixture: {newColorCard?.ColorMixture}, CreatedDate: {newColorCard?.CreatedDate}");
                await _dbOperations.ColorCards.AddAsync(newColorCard);
                await _dbOperations.SaveChangesAsync();
                Debug.WriteLine($"Added ColorCard with Id: {newColorCard?.Id}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding ColorCard: {ex.Message}");
            }
        }

        public async Task<ColorCard> GetColorCardAsync(int id)
        {
            ColorCard retrievedColorCard = await _dbOperations.ColorCards.FirstOrDefaultAsync(cc => cc.Id == id);
            Debug.WriteLine($"Retrieved ColorCard: Id:{retrievedColorCard?.Id}, ColorMixture: {retrievedColorCard?.ColorMixture}, CreatedDate: {retrievedColorCard?.CreatedDate}");
            return retrievedColorCard;
        }

        public async Task<IEnumerable<ColorCard>> GetColorCardsByClientAsync(int clientId)
        {
            IEnumerable<ColorCard> retrievedColorCards = await _dbOperations.ColorCards
                .Where(cc => cc.Client != null && cc.Client.Id == clientId)
                .ToListAsync();
            Debug.WriteLine($"Retrieved {retrievedColorCards.Count()} ColorCards for client Id: {clientId}");
            foreach (var colorCard in retrievedColorCards)
            {
                Debug.WriteLine($"ColorCard Id: {colorCard.Id}, ColorMixture: {colorCard.ColorMixture}, CreatedDate: {colorCard.CreatedDate}");
            }
            return retrievedColorCards;
        }

        public async Task UpdateColorCardAsync(ColorCard updatedColorCard)
        {
            _dbOperations.ColorCards.Update(updatedColorCard);
            await _dbOperations.SaveChangesAsync();
        }

        public async Task DeleteColorCardAsync(int id)
        {
            var colorCardToDelete = await _dbOperations.ColorCards.FirstOrDefaultAsync(cc => cc.Id == id);
            if (colorCardToDelete != null)
            {
                _dbOperations.ColorCards.Remove(colorCardToDelete);
                await _dbOperations.SaveChangesAsync();
            }
            else
            {
                Debug.WriteLine("Color card not found for deletion.");
            }
        }
    }
}
