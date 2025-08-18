using HIDAeroService.Data;
using HIDAeroService.Entity;

namespace HIDAeroService.Service
{
    public class CardFormatService
    {
        private readonly ILogger<CardFormatService> _logger;
        private readonly AppDbContext _context;
        public CardFormatService(AppDbContext context,ILogger<CardFormatService> logger)
        {
            _logger = logger;
            _context = context;
        }

        public List<ar_card_format> GetCardFormatList()
        {
            return _context.ar_card_formats.ToList();
        }


    }
}
