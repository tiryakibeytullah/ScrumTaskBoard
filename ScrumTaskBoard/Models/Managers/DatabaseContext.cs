using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ScrumTaskBoard.Models.Managers
{
    public class DatabaseContext : DbContext
    {
        public DbSet<TeknikKart> TeknikKart { get; set; }
        public DbSet<Is> Is { get; set; }
        public DbSet<Durum> Durum { get; set; }

        public DatabaseContext()
        {
            Database.SetInitializer(new VeritabanıOlusturucu());
        }
    }

    public class VeritabanıOlusturucu : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            // Teknik kart ekleme.
            for (int i = 0; i < 2; i++)
            {
                TeknikKart kart = new TeknikKart();
                kart.projeAdi = FakeData.NameData.GetCompanyName();
                kart.teknikUzmanAdı = FakeData.NameData.GetFullName();
                kart.tarih = FakeData.DateTimeData.GetDatetime();
                kart.tahminiSure = FakeData.NumberData.GetNumber(1, 10);
                kart.gerceklesenSure = FakeData.NumberData.GetNumber(1, 10);
                kart.isinAciklamasi = "Proje için yapılacak olan iş açıklaması.";
                kart.notlar = "Proje için yapılacak olan notlar.";

                context.TeknikKart.Add(kart);
            }
            context.SaveChanges();

            //Teknik kart'a 2 adet iş takip ekleme.
            List<TeknikKart> tumKartlar = context.TeknikKart.ToList();

            foreach (TeknikKart kart in tumKartlar)
            {
                for (int i = 0; i < 2; i++)
                {
                    Is newIs = new Is();
                    newIs.tarih = FakeData.DateTimeData.GetDatetime();
                    newIs.durum= 1;
                    newIs.yapilacak_Is = "Temizlik";
                    newIs.aciklama = "Tüm katların temizliği yapılacak.";
                    newIs.teknikKart_Id = kart.teknikKart_Id;

                    context.Is.Add(newIs);
                }
            }
            context.SaveChanges();

            Durum durumEkle = new Durum();
            durumEkle.DurumBilgisi = "Yapılacak";
            context.Durum.Add(durumEkle);
            context.SaveChanges();

            durumEkle.DurumBilgisi = "Yapılıyor";
            context.Durum.Add(durumEkle);
            context.SaveChanges();

            durumEkle.DurumBilgisi = "Tamamlandı";
            context.Durum.Add(durumEkle);

            context.SaveChanges();
        }
    }
}