using WinterIntex.Infrastructure;
using WinterIntex.Models;
using System.Text.Json.Serialization;


namespace WinterIntex.Models
{
    public class SessionCart : LineItems
    {

        public static LineItems GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()
                .HttpContext?.Session;

            SessionCart cart = session?.GetJson<SessionCart>("Cart") ??
                new SessionCart();

            cart.Session = session;

            return cart;
        }

        [JsonIgnore]
        public ISession? Session { get; set; }

        public override void AddItem(Product p, int quantity)
        {
            base.AddItem(p, quantity);
            Session?.SetJson("Cart", this);
        }

        public override void RemoveLine(Product proj)
        {
            base.RemoveLine(proj);
            Session?.SetJson("Cart", this);
        }

        public override void Clear()
        {
            base.Clear();
            Session?.Remove("Cart");
        }
    }
}
