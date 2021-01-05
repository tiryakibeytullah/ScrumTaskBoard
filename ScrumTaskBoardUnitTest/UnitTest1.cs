using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ScrumTaskBoardUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        IWebDriver driver = new InternetExplorerDriver();
        public void SiteyeGiris()
        {
            // Sayfaya giriş için kullanılır.
            driver.Navigate().GoToUrl("https://localhost:44354/Home/HomePage");
        }
        public void SiteyiKapat()
        {
            driver.Close();
        }
        [TestMethod]
        public void TaskAddControl()
        {
            // Ana sayfaya giriş yaptıktan sonra, kart ekleme butonun tıklayarak gerekli alanlar doldurulur ve kart eklenir. Kart ekleme sonucu sayfanın alt kısmında bir adet alert belirir. Bu işlem başarılır ise alert'in mesajı 'Kart ekleme işlemi gerçekleşmiştir.' olacaktır. Aksi taktirde alert'in mesajı 'Kart ekleme işleminde hata oluştu.'ile yanıt verecektir. Bu testte eklenen kartın doğru cevap vermediği kontrol edilmektedir.
            SiteyeGiris();
            driver.FindElement(By.Id("btnKartEkle")).Click();
            driver.FindElement(By.Id("txtTarih")).SendKeys("2.01.2021");
            driver.FindElement(By.Id("txtProjeAdi")).SendKeys("Selenium Deneme-2 Proje");
            driver.FindElement(By.Id("txtTeknikUzmanAdi")).SendKeys("Habil Yadigar");
            driver.FindElement(By.Id("txtTahminiSure")).SendKeys("10");
            driver.FindElement(By.Id("txtGerceklesenSure")).SendKeys("5");
            driver.FindElement(By.Id("txtAreaIsAciklama")).SendKeys("Selenium Açıklama 2");
            driver.FindElement(By.Id("txtAreaNotlar")).SendKeys("Selenium Not 2");
            driver.FindElement(By.Id("btnYeniKartEkle")).Click();

            IWebElement elemnt = driver.FindElement(By.Id("teknikKartAlert"));
            string innerText = elemnt.Text;
            Assert.AreEqual("Kart ekleme işlemi gerçekleşmiştir.", innerText);

            SiteyiKapat();
        }
        [TestMethod]
        public void IsAddControl()
        {
            // Ana sayfaya giriş yapılıp İş ekleme butonuna tıklanarak gerekli alanlar doldurulduktan sonra iş eklenmektedir. İş ekleme sonucu alert mesajına göre işlemin gerçekleşip gerçekleşmediği anlaşılır.
            SiteyeGiris();
            driver.FindElement(By.Id("btnIsEkle")).Click();

            var select = driver.FindElement(By.Name("teknikKart_Id"));
            var selectElement = new SelectElement(select);

            selectElement.SelectByValue("8");
            selectElement.SelectByText("8 - Selenium Deneme-2 Proje / Habil Yadigar");

            driver.FindElement(By.Id("txtTarih")).SendKeys("1.01.2021");
            driver.FindElement(By.Id("txtAreaYapilacakIs")).SendKeys("Selenium iş için gerekli açıklama 2");
            driver.FindElement(By.Id("txtAreaAciklama")).SendKeys("Selenium gerekli açıklama 2");
            driver.FindElement(By.Id("btnYeniIsEkle")).Click();

            IWebElement elemnt = driver.FindElement(By.Id("resultIsAlert"));
            string innerText = elemnt.Text;
            Assert.AreEqual("Yeni iş takip planı kaydedilmiştir.", innerText);

            SiteyiKapat();
        }
        [TestMethod]
        public void TaskUpdateControl()
        {
            // Ana sayfaya giriş yaptıktan sonra 'Selenium Deneme Proje' adlı kartı görüntüleme butonuna basılır. Kart sayfası açıldıktan sonra, notlar kısmı silinerek yeni veri eklenir ve güncelleme işlemi gerçekleşir.
            SiteyeGiris();
            driver.FindElement(By.Id("teknikKart6")).Click();
            driver.FindElement(By.Id("txtTarih")).Clear();
            driver.FindElement(By.Id("txtTarih")).SendKeys("1.01.2021");
            driver.FindElement(By.Id("txtAreaIsAciklama")).Clear();
            driver.FindElement(By.Id("txtAreaIsAciklama")).SendKeys("Selenium ile not ekleme testi yapıldı.");
            driver.FindElement(By.Id("btnKartDuzenle")).Click();

            IWebElement element = driver.FindElement(By.Id("teknikKartDuzenleAlert"));
            string elementText = element.Text;
            Assert.AreEqual("Kart güncelleme işlemi gerçekleşmiştir.", elementText);

            SiteyiKapat();
        }
    }
}
