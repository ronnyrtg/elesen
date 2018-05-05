namespace TradingLicense.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TradingLicense.Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<TradingLicense.Data.LicenseApplicationContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "TradingLicense.Data.LicenseApplicationContext";
        }

        protected override void Seed(TradingLicense.Data.LicenseApplicationContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var sector = new List<Sector>
            {
                new Sector {SectorID=1,SectorDesc="Tred"},
                new Sector {SectorID=2,SectorDesc="Stor"},
                new Sector {SectorID=3,SectorDesc="Perindustrian"},
                new Sector {SectorID=4,SectorDesc="Bengkel"},
                new Sector {SectorID=5,SectorDesc="Pertanian/Penternakan"},
                new Sector {SectorID=6,SectorDesc="Lain-lain"},
                new Sector {SectorID=7,SectorDesc="Makanan/Minuman"},
                new Sector {SectorID=8,SectorDesc="Hotel"},
                new Sector {SectorID=9,SectorDesc="Barang Lusuh/Scrap"},
            };
            sector.ForEach(s => context.Sectors.Add(s));
            context.SaveChanges();

            var businesstypes = new List<BusinessType>
            {
                new BusinessType {BusinessTypeCode="I",BusinessTypeDesc="HAK MILIK PERSEORANGAN" },
                new BusinessType {BusinessTypeCode="O",BusinessTypeDesc="LAIN-LAIN" },
                new BusinessType {BusinessTypeCode="P",BusinessTypeDesc="PERKONGSIAN" },
                new BusinessType {BusinessTypeCode="U",BusinessTypeDesc="SYARIKAT AWAM BERHAD" },
                new BusinessType {BusinessTypeCode="C",BusinessTypeDesc="SYARIKAT KERJASAMA" },
                new BusinessType {BusinessTypeCode="R",BusinessTypeDesc="SYARIKAT SENDIRIAN BERHAD" },
            };
            businesstypes.ForEach(s => context.BusinessTypes.Add(s));
            context.SaveChanges();

            var businesscode = new List<BusinessCode>
            {
                new BusinessCode {CodeNumber="A001",SectorID=1,CodeDesc="Pejabat urusan",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A002",SectorID=1,CodeDesc="Kemudahan dan perkhidmatan jagaan kesihatan swasta",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A003",SectorID=1,CodeDesc="Bank dan institusi kewangan",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A004",SectorID=1,CodeDesc="Kedai buku dan alat tulis",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A005",SectorID=1,CodeDesc="Institusi pendidikan tinggi swasta, sekolah swasta atau institusi pendidikan swasta",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A006",SectorID=1,CodeDesc="Barangan elektrik atau elektronik atau komputer",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A007",SectorID=1,CodeDesc="Perabot",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A008",SectorID=1,CodeDesc="Hiasan dalaman/barangan hiasan dalaman",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A009",SectorID=1,CodeDesc="Peralatan dan perkakasan rumah/pejabat",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A010",SectorID=1,CodeDesc="Peralatan dan kelengkapan dapur",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A011",SectorID=1,CodeDesc="Perkhidmatan menjahit",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A012",SectorID=1,CodeDesc="Pakaian, tekstil dan alat jahitan",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A013",SectorID=1,CodeDesc="Kosmetik dan kelengkapan dandanan diri",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A014",SectorID=1,CodeDesc="Peralatan makmal, saintifik dan perubatan",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A015",SectorID=1,CodeDesc="Ubat, farmasi dan produk kesihatan",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A016",SectorID=1,CodeDesc="Barang kemas, hiasan diri dan persendirian",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A017",SectorID=1,CodeDesc="Jam",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A018",SectorID=1,CodeDesc="Bagasi",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A019",SectorID=1,CodeDesc="Cenderamata",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A020",SectorID=1,CodeDesc="Barangan optik",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A021",SectorID=1,CodeDesc="Peralatan, aksesori dan perkhidmatan fotografi",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A022",SectorID=1,CodeDesc="Bunga dan tumbuhan tiruan/hidup",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A023",SectorID=1,CodeDesc="Perkhidmatan andaman dan pakaian pengantin",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A024",SectorID=1,CodeDesc="Telefon dan aksesori",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A025",SectorID=1,CodeDesc="Barangan antik",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A026",SectorID=1,CodeDesc="Menjilid buku atau membuat fotokopi",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A027",SectorID=1,CodeDesc="Tukang kunci",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A028",SectorID=1,CodeDesc="Pusat/studio rakaman audio",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A029",SectorID=1,CodeDesc="Stesen minyak/gas/elektrik",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A030",SectorID=1,CodeDesc="Media digital/elektronik dan aksesori berkaitan",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A031",SectorID=1,CodeDesc="Barangan logam (untuk sektor pembinaan dan pembuatan)",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A032",SectorID=1,CodeDesc="Alat-alat muzik atau kelengkapan muzik",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A033",SectorID=1,CodeDesc="Kelas kesenian/kebudayaan/kemahiran",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A034",SectorID=1,CodeDesc="Peralatan, bahan dan hiasan landskap",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A035",SectorID=1,CodeDesc="Kenderaan berat",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A036",SectorID=1,CodeDesc="Kereta, motosikal, bot dan jet ski",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A037",SectorID=1,CodeDesc="Mesin dan jentera",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A038",SectorID=1,CodeDesc="Basikal dan aksesori",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A039",SectorID=1,CodeDesc="Peralatan kesihatan, kecergasan dan sukan",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A040",SectorID=1,CodeDesc="Produk berasaskan tembakau (seperti rokok/curut dan produk seumpamanya)",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A041",SectorID=1,CodeDesc="Kemasan bangunan",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A042",SectorID=1,CodeDesc="Alat permainan dan barangan hobi",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A043",SectorID=1,CodeDesc="Percetakan digital",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A044",SectorID=1,CodeDesc="Kedai barangan runcit/kedai serbaneka (seperti pasar raya dan gedung)",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A045",SectorID=1,CodeDesc="Sewaan kereta",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A046",SectorID=1,CodeDesc="Haiwan peliharaan, dandanan binatang, peralatan dan makanan haiwan dan/atau rumah tumpangan haiwan",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A047",SectorID=1,CodeDesc="Agensi teman sosial",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A048",SectorID=1,CodeDesc="Kedai bebas cukai",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A049",SectorID=1,CodeDesc="Pengurusan mayat dan pengebumian",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A050",SectorID=1,CodeDesc="Baja, racun atau kimia-kimia lain yang serupa",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A051",SectorID=1,CodeDesc="Bahan berbahaya yang mudah terbakar tidak termasuk petroleum dan gas",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A052",SectorID=1,CodeDesc="Dobi",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A053",SectorID=1,CodeDesc="Stor kayu",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A054",SectorID=1,CodeDesc="Agensi pelancongan",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A055",SectorID=1,CodeDesc="Agensi pekerjaan",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A056",SectorID=1,CodeDesc="Jualan tiket pengangkutan awam",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A057",SectorID=1,CodeDesc="Pusat kecantikan dan penjagaan kesihatan",DefaultRate=25f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A058",SectorID=1,CodeDesc="Joki kereta",DefaultRate=0.0f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="A059",SectorID=1,CodeDesc="Pemberi Pinjam Wang",DefaultRate=0.0f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=2},
                new BusinessCode {CodeNumber="B001",SectorID=2,CodeDesc="Gudang/stor makanan",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="B002",SectorID=2,CodeDesc="Gudang/stor barang-barang lain",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="B003",SectorID=2,CodeDesc="Gudang/stor bahan merbahaya",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C001",SectorID=3,CodeDesc="Barang-barang berasaskan logam",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C002",SectorID=3,CodeDesc="Media digital dan elektronik",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C003",SectorID=3,CodeDesc="Makanan dan perkakasan haiwan",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C004",SectorID=3,CodeDesc="Baja",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C005",SectorID=3,CodeDesc="Kenderaan, jentera dan mesin",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C006",SectorID=3,CodeDesc="Alat-alat ganti dan aksesori kenderaan bermotor",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C007",SectorID=3,CodeDesc="Basikal",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C008",SectorID=3,CodeDesc="Tayar dan tiub",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C009",SectorID=3,CodeDesc="Bahan binaan",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C010",SectorID=3,CodeDesc="Bateri",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C011",SectorID=3,CodeDesc="Kabel dan wayar",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C012",SectorID=3,CodeDesc="Permaidani dan hamparan",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C013",SectorID=3,CodeDesc="Keranda dan batu nisan",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C014",SectorID=3,CodeDesc="Kosmetik dan kelengkapan dandanan diri",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C015",SectorID=3,CodeDesc="Bahan pencuci, alat-alat mencuci bahan-bahan lain yang seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C016",SectorID=3,CodeDesc="Bahan pengilap",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C017",SectorID=3,CodeDesc="Mengisi gas ke dalam botol atau silinder",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C018",SectorID=3,CodeDesc="Dadah/ubat-ubatan dan keluaran-keluaran farmasi",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C019",SectorID=3,CodeDesc="Fabrik/kulit atau pakaian",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C020",SectorID=3,CodeDesc="Menjahit dan menyulam",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C021",SectorID=3,CodeDesc="Barang-barang elektrik dan elektronik",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C022",SectorID=2,CodeDesc="Barang-barang komputer",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C023",SectorID=2,CodeDesc="Membuat barang-barang perubatan, saintifik dan makmal",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C024",SectorID=2,CodeDesc="Kaca dan cermin",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C025",SectorID=2,CodeDesc="Anggota badan palsu",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C026",SectorID=2,CodeDesc="Produk dibuat daripada gelas serabut, gentian sintetik, kapas tali dan produk seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C027",SectorID=2,CodeDesc="Mercun dan bahan letupan",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C028",SectorID=2,CodeDesc="Gas mudah terbakar",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C029",SectorID=3,CodeDesc="Keluaran petroleum termasuk minyak pelincir dan lain-lain minyak yang seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C030",SectorID=3,CodeDesc="Produk berasaskan emas, perak, tembaga dan bahan-bahan yang seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C031",SectorID=3,CodeDesc="Peralatan sembahyang dan barang-barang lain yang berkaitan",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C032",SectorID=3,CodeDesc="Batu kapur",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C033",SectorID=3,CodeDesc="Cat",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C034",SectorID=3,CodeDesc="Kertas dan hasil-hasil kertas",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C035",SectorID=3,CodeDesc="Plastik, barang-barang daripada bahan plastik atau bahan lain yang seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C036",SectorID=3,CodeDesc="Kaca, barang-barang daripada bahan kaca atau bahan lain yang seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C037",SectorID=3,CodeDesc="Logam, barang-barang daripada bahan logam atau bahan lain yang seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C038",SectorID=3,CodeDesc="Papan dan kayu",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C039",SectorID=3,CodeDesc="Perabot",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C040",SectorID=3,CodeDesc="Barang-barang tembikar",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C041",SectorID=3,CodeDesc="Percetakan (berskala besar)",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C042",SectorID=3,CodeDesc="Pam dan penapis air",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C043",SectorID=3,CodeDesc="Keluaran getah",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C044",SectorID=3,CodeDesc="Tirai dan bidai",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C045",SectorID=3,CodeDesc="Alat tulis/buku/majalah",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C046",SectorID=3,CodeDesc="Barang-barang mainan",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C047",SectorID=3,CodeDesc="Barangan pertanian dan kimia industri",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C048",SectorID=3,CodeDesc="Blok ais",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C049",SectorID=3,CodeDesc="Visual iklan",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C050",SectorID=3,CodeDesc="Makanan dan seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="C051",SectorID=3,CodeDesc="Minuman dan seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="D001",SectorID=4,CodeDesc="Alat ganti dan aksesori",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="D002",SectorID=4,CodeDesc="Pemasangan penyaman udara di kenderaan",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="D003",SectorID=4,CodeDesc="Kenderaan bermotor dan kenderaan marin",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="D004",SectorID=4,CodeDesc="Tayar",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="D005",SectorID=4,CodeDesc="Mencuci dan/atau mengilap kereta",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="D006",SectorID=4,CodeDesc="Menyembur cat, selulosa dan bahan-bahan kimia lain",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="D007",SectorID=4,CodeDesc="Kerja-kerja kejuruteraan",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="D008",SectorID=4,CodeDesc="Kerja-kerja kimpalan",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="D009",SectorID=4,CodeDesc="Pertukangan batu, kayu, kaca dan logam (termasuk papan iklan) ",DefaultRate=2.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="E001",SectorID=5,CodeDesc="Menternak burung walit",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="E002",SectorID=5,CodeDesc="Menternak lebah, lintah, cacing dan seumpamanya",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="E003",SectorID=5,CodeDesc="Tempat pembiakan haiwan",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="E004",SectorID=5,CodeDesc="Rumah sembelihan binatang",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="E005",SectorID=5,CodeDesc="Kolam pancing",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="E006",SectorID=5,CodeDesc="Semaian tumbuhan",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="F001",SectorID=6,CodeDesc="Mana-mana aktiviti perniagaan yang tidak termasuk dalam Jadual",DefaultRate=1.5f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="G001",SectorID=7,CodeDesc="Restoran/kedai makan/gerai makan/kios makanan",DefaultRate=10.0f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="G002",SectorID=7,CodeDesc="Menjual makanan/minuman (tanpa tempat makan)",DefaultRate=10.0f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="G003",SectorID=7,CodeDesc="Katering makanan",DefaultRate=10.0f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="G004",SectorID=7,CodeDesc="Kantin sekolah",DefaultRate=10.0f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="G005",SectorID=7,CodeDesc="Kantin pejabat",DefaultRate=10.0f,BaseFee=0.0f,ExtraFee=0.0f,ExtraUnit=0,Period="Y",PQuantity=1},
                new BusinessCode {CodeNumber="H001",SectorID=8,CodeDesc="Hotel (Kelas Pertama)",DefaultRate=0.0f,BaseFee=150.0f,ExtraFee=0.0f,ExtraUnit=0,Period="M",PQuantity=1},
                new BusinessCode {CodeNumber="H002",SectorID=8,CodeDesc="Hotel (Kelas Kedua)",DefaultRate=0.0f,BaseFee=100.0f,ExtraFee=0.0f,ExtraUnit=0,Period="M",PQuantity=1},
                new BusinessCode {CodeNumber="H003",SectorID=8,CodeDesc="Hotel (Kelas Ketiga)",DefaultRate=0.0f,BaseFee=30.0f,ExtraFee=0.0f,ExtraUnit=0,Period="M",PQuantity=1},
                new BusinessCode {CodeNumber="H004",SectorID=8,CodeDesc="Lodging House/Rumah Tumpangan",DefaultRate=0.0f,BaseFee=30.0f,ExtraFee=0.0f,ExtraUnit=0,Period="M",PQuantity=1},
                new BusinessCode {CodeNumber="I001",SectorID=9,CodeDesc="Barang Lupus/Dealing in Scrap",DefaultRate=0.0f,BaseFee=25.0f,ExtraFee=0.0f,ExtraUnit=0,Period="M",PQuantity=3},
            };
            businesscode.ForEach(s => context.BusinessCodes.Add(s));
            context.SaveChanges();

            var departments = new List<Department>
            {
                new Department {DepartmentCode="Lesen",DepartmentDesc="Bahagian Pelesenan PL",Internal=true},
                new Department {DepartmentCode="ICT",DepartmentDesc="Bahagian ICT PL",Internal=true},
                new Department {DepartmentCode="Harta",DepartmentDesc="Bahagian Harta PL",Internal=true},
                new Department {DepartmentCode="Kewangan",DepartmentDesc="Bahagian Kewangan PL",Internal=true},
                new Department {DepartmentCode="PKPE",DepartmentDesc="Pejabat Ketua Pegawai Eksekutif PL",Internal=true},
                new Department {DepartmentCode="Bomba",DepartmentDesc="Jabatan Bomba & Penyelamat Malaysia",Internal=false},
                new Department {DepartmentCode="KPDNKK",DepartmentDesc="Kementerian Perdagangan Dalam Negeri, Koperasi & Kepenggunaan",Internal=false},
                new Department {DepartmentCode="JKKP",DepartmentDesc="Jabatan Keselamatan dan Kesihatan Pekerjaan",Internal=false},
                new Department {DepartmentCode="ST",DepartmentDesc="Suruhanjaya Tenaga",Internal=false},
                new Department {DepartmentCode="MPIC",DepartmentDesc="Kementerian Perusahaan Perladangan dan Komoditi",Internal=false},
                new Department {DepartmentCode="MPOB",DepartmentDesc="Lembaga Minyak Sawit Malaysia",Internal=false},
                new Department {DepartmentCode="LKTN",DepartmentDesc="Lembaga Kenaf dan Tembakau Negara",Internal=false},
                new Department {DepartmentCode="LGM",DepartmentDesc="Lembaga Getah Malaysia",Internal=false},
                new Department {DepartmentCode="JPJ",DepartmentDesc="Jabatan Pengangkutan Jalan",Internal=false},
                new Department {DepartmentCode="JAS",DepartmentDesc="Jabatan Alam Sekitar",Internal=false},
                new Department {DepartmentCode="PPN",DepartmentDesc="Pejabat Perhutanan Negeri",Internal=false},
                new Department {DepartmentCode="BPFK",DepartmentDesc="Biro Pengawalan Farmaseutikal Kebangsaan",Internal=false},
                new Department {DepartmentCode="BKKM",DepartmentDesc="Bahagian Keselamatan Dan Kualiti Makanan",Internal=false},
                new Department {DepartmentCode="MOA",DepartmentDesc="Kementerian Pertanian Malaysia",Internal=false},
                new Department {DepartmentCode="JPV",DepartmentDesc="Jabatan Perkhidmatan Veterinar",Internal=false},
                new Department {DepartmentCode="LPNM",DepartmentDesc="Lembaga Perindustrian Nanas Malaysia",Internal=false},
                new Department {DepartmentCode="PDRM",DepartmentDesc="Polis Diraja Malaysia",Internal=false},
                new Department {DepartmentCode="LKM",DepartmentDesc="Lembaga Koko Malaysia",Internal=false},
                new Department {DepartmentCode="PERHILITAN",DepartmentDesc="Jabatan Perlindungan Hidupan Liar dan Taman Negara",Internal=false},
                new Department {DepartmentCode="FAMA",DepartmentDesc="Lembaga Pemasaran Pertanian Persekutuan",Internal=false},
                new Department {DepartmentCode="FINAS",DepartmentDesc="Perbadanan Kemajuan Filem Nasional Malaysia",Internal=false},
                new Department {DepartmentCode="LFM",DepartmentDesc="Lembaga Farmasi Malaysia",Internal=false},
                new Department {DepartmentCode="MOM",DepartmentDesc="Majlis Optik Malaysia",Internal=false},
                new Department {DepartmentCode="JPM",DepartmentDesc="Jabatan Pertanian Malaysia",Internal=false},
                new Department {DepartmentCode="MOTAC",DepartmentDesc="Kementerian Pelancongan dan Kebudayaan Malaysia",Internal=false},
                new Department {DepartmentCode="KPKT",DepartmentDesc="Kementerian Perumahan dan Kerajaan Tempatan",Internal=false},
                new Department {DepartmentCode="JTK",DepartmentDesc="Jabatan Tenaga Kerja",Internal=false},
                new Department {DepartmentCode="KDN",DepartmentDesc="Kementerian Dalam Negeri",Internal=false},
                new Department {DepartmentCode="BEM",DepartmentDesc="Lembaga Jurutera Malaysia",Internal=false},
                new Department {DepartmentCode="CIDB",DepartmentDesc="Lembaga Pembangunan Industri Pembinaan Malaysia",Internal=false},
                new Department {DepartmentCode="MOE",DepartmentDesc="Kementerian Pendidikan",Internal=false},
                new Department {DepartmentCode="MOHE",DepartmentDesc="Kementerian Pendidikan Tinggi",Internal=false},
                new Department {DepartmentCode="DCA",DepartmentDesc="Jabatan Penerbangan Awam Malaysia",Internal=false},
                new Department {DepartmentCode="JKM",DepartmentDesc="Jabatan Kebajikan Masyarakat",Internal=false},
                new Department {DepartmentCode="CKAPS",DepartmentDesc="Cawangan Kawalan Amalan Perubatan Swasta",Internal=false},
                new Department {DepartmentCode="MOH",DepartmentDesc="Kementerian Kesihatan Malaysia",Internal=false},
                new Department {DepartmentCode="MDC",DepartmentDesc="Majlis Pergigian Malaysia",Internal=false},
                new Department {DepartmentCode="JKDM",DepartmentDesc="Jabatan Kastam Diraja Malaysia",Internal=false},
                new Department {DepartmentCode="BNM",DepartmentDesc="Bank Negara Malaysia",Internal=false},
                new Department {DepartmentCode="SKMM",DepartmentDesc="Suruhanjaya Komunikasi dan Multimedia Malaysia",Internal=false},
                new Department {DepartmentCode="SPAD",DepartmentDesc="Suruhanjaya Pengangkutan Awam Darat",Internal=false},
                new Department {DepartmentCode="JPSPN",DepartmentDesc="Jabatan Pengurusan Sisa Pepejal Negara dan Pembersihan Awam",Internal=false},
                new Department {DepartmentCode="MOF",DepartmentDesc="Kementerian Kewangan",Internal=false},
            };
            departments.ForEach(s => context.Departments.Add(s));
            context.SaveChanges();

            var premisetypes = new List<PremiseType>
            {
                new PremiseType {PremiseDesc="Rumah Kedai/ Kedai Pejabat/ Pusat Perniagaan Komersil"},
                new PremiseType {PremiseDesc="Kompleks Beli Belah"},
                new PremiseType {PremiseDesc="Kilang, Bengkel, Hotel"},
                new PremiseType {PremiseDesc="SOHO/ SOFO"},
                new PremiseType {PremiseDesc="Kompleks Pembangunan Bercampur"},
                new PremiseType {PremiseDesc="Lain-lain"},
            };
            premisetypes.ForEach(s => context.PremiseTypes.Add(s));
            context.SaveChanges();

            var individuals = new List<Individual>
            {
                new Individual{FullName="Ali Bin Abu",MykadNo="710213-12-4820",NationalityID=1,PhoneNo="0108103140",AddressIC="No.3, Kg. Tg. Aru, Jalan Tg. Aru, 87000 W.P.Labuan",IndividualEmail="aliabu@yahoo.com",Gender=1,Rental=0.10f,Assessment=10.0f,Compound=2.5f},
                new Individual{FullName="Siti Aminah",MykadNo="610122-12-4933",NationalityID=1,PhoneNo="0112546778",AddressIC="Lot 20, Blok F, Taman Mutiara, 87000 W.P.Labuan",IndividualEmail="sitiaminah@gmail.com",Gender=2,Rental=0.0f,Assessment=0.0f,Compound=0.0f},
                new Individual{FullName="Chin Chee Kiong",MykadNo="500101-12-5129",NationalityID=1,PhoneNo="0148552370",AddressIC="Lot 13, Blok D, Jalan Merdeka, Pusat Bandar, 87000 W.P.Labuan",IndividualEmail="chinchee70@gmail.com",Gender=1,Rental=100.0f,Assessment=0.25f,Compound=0.0f},
            };
            individuals.ForEach(s => context.Individuals.Add(s));
            context.SaveChanges();

            var companies = new List<Company>
            {
                new Company {RegistrationNo="75278-T",CompanyName="Chin Recycle",CompanyAddress="Lot 12-F, Blok 20, Jalan Tenaga, Labuan"},
                new Company {RegistrationNo="801234-V",CompanyName="Kejora Bersatu Sdn Bhd",CompanyAddress="No.7, 1st Floor, Financial Park, Jalan Merdeka, 87000 Labuan"},
            };
            companies.ForEach(s => context.Companies.Add(s));
            context.SaveChanges();

            var indlinkcoms = new List<IndLinkCom>
            {
                new IndLinkCom {IndividualID=1,CompanyID=2 },
                new IndLinkCom {IndividualID=2,CompanyID=2 },
                new IndLinkCom {IndividualID=3,CompanyID=1 },
            };
            indlinkcoms.ForEach(s => context.IndLinkComs.Add(s));
            context.SaveChanges();


            var roletemplates = new List<RoleTemplate>
            {
                new RoleTemplate {RoleTemplateDesc="Public" },
                new RoleTemplate {RoleTemplateDesc="Desk Officer"},
                new RoleTemplate {RoleTemplateDesc="Clerk"},
                new RoleTemplate {RoleTemplateDesc="Supervisor"},
                new RoleTemplate {RoleTemplateDesc="Route Unit" },
                new RoleTemplate {RoleTemplateDesc="Director"},
                new RoleTemplate {RoleTemplateDesc="CEO" },
                new RoleTemplate {RoleTemplateDesc="Administrator"},
            };
            roletemplates.ForEach(s => context.RoleTemplates.Add(s));
            context.SaveChanges();

            var accesspages = new List<AccessPage>
            {
                new AccessPage {PageDesc="AccessPages",CrudLevel=0,RoleTemplateID=1,ScreenId=1},
                new AccessPage {PageDesc="AccessPages",CrudLevel=0,RoleTemplateID=2,ScreenId=1},
                new AccessPage {PageDesc="AccessPages",CrudLevel=2,RoleTemplateID=3,ScreenId=1},
                new AccessPage {PageDesc="AccessPages",CrudLevel=3,RoleTemplateID=4,ScreenId=1},
                new AccessPage {PageDesc="AccessPages",CrudLevel=3,RoleTemplateID=5,ScreenId=1},
                new AccessPage {PageDesc="AccessPages",CrudLevel=4,RoleTemplateID=6,ScreenId=1},
                new AccessPage {PageDesc="AccessPages",CrudLevel=4,RoleTemplateID=7,ScreenId=1},
                new AccessPage {PageDesc="AccessPages",CrudLevel=4,RoleTemplateID=8,ScreenId=1},

                new AccessPage {PageDesc="AdditionalInfos",CrudLevel=0,RoleTemplateID=1,ScreenId=2},
                new AccessPage {PageDesc="AdditionalInfos",CrudLevel=0,RoleTemplateID=2,ScreenId=2},
                new AccessPage {PageDesc="AdditionalInfos",CrudLevel=2,RoleTemplateID=3,ScreenId=2},
                new AccessPage {PageDesc="AdditionalInfos",CrudLevel=3,RoleTemplateID=4,ScreenId=2},
                new AccessPage {PageDesc="AdditionalInfos",CrudLevel=3,RoleTemplateID=5,ScreenId=2},
                new AccessPage {PageDesc="AdditionalInfos",CrudLevel=4,RoleTemplateID=6,ScreenId=2},
                new AccessPage {PageDesc="AdditionalInfos",CrudLevel=4,RoleTemplateID=7,ScreenId=2},
                new AccessPage {PageDesc="AdditionalInfos",CrudLevel=4,RoleTemplateID=8,ScreenId=2},

                new AccessPage {PageDesc="Attachment",CrudLevel=0,RoleTemplateID=1,ScreenId=3},
                new AccessPage {PageDesc="Attachment",CrudLevel=0,RoleTemplateID=2,ScreenId=3},
                new AccessPage {PageDesc="Attachment",CrudLevel=2,RoleTemplateID=3,ScreenId=3},
                new AccessPage {PageDesc="Attachment",CrudLevel=3,RoleTemplateID=4,ScreenId=3},
                new AccessPage {PageDesc="Attachment",CrudLevel=3,RoleTemplateID=5,ScreenId=3},
                new AccessPage {PageDesc="Attachment",CrudLevel=4,RoleTemplateID=6,ScreenId=3},
                new AccessPage {PageDesc="Attachment",CrudLevel=4,RoleTemplateID=7,ScreenId=3},
                new AccessPage {PageDesc="Attachment",CrudLevel=4,RoleTemplateID=8,ScreenId=3},

                new AccessPage {PageDesc="Administrator",CrudLevel=0,RoleTemplateID=1,ScreenId=4},
                new AccessPage {PageDesc="Administrator",CrudLevel=0,RoleTemplateID=2,ScreenId=4},
                new AccessPage {PageDesc="Administrator",CrudLevel=2,RoleTemplateID=3,ScreenId=4},
                new AccessPage {PageDesc="Administrator",CrudLevel=3,RoleTemplateID=4,ScreenId=4},
                new AccessPage {PageDesc="Administrator",CrudLevel=3,RoleTemplateID=5,ScreenId=4},
                new AccessPage {PageDesc="Administrator",CrudLevel=4,RoleTemplateID=6,ScreenId=4},
                new AccessPage {PageDesc="Administrator",CrudLevel=4,RoleTemplateID=7,ScreenId=4},
                new AccessPage {PageDesc="Administrator",CrudLevel=4,RoleTemplateID=8,ScreenId=4},

                new AccessPage {PageDesc="MasterSetup",CrudLevel=0,RoleTemplateID=1,ScreenId=5},
                new AccessPage {PageDesc="MasterSetup",CrudLevel=0,RoleTemplateID=2,ScreenId=5},
                new AccessPage {PageDesc="MasterSetup",CrudLevel=2,RoleTemplateID=3,ScreenId=5},
                new AccessPage {PageDesc="MasterSetup",CrudLevel=3,RoleTemplateID=4,ScreenId=5},
                new AccessPage {PageDesc="MasterSetup",CrudLevel=3,RoleTemplateID=5,ScreenId=5},
                new AccessPage {PageDesc="MasterSetup",CrudLevel=4,RoleTemplateID=6,ScreenId=5},
                new AccessPage {PageDesc="MasterSetup",CrudLevel=4,RoleTemplateID=7,ScreenId=5},
                new AccessPage {PageDesc="MasterSetup",CrudLevel=4,RoleTemplateID=8,ScreenId=5},

                new AccessPage {PageDesc="Inquiry",CrudLevel=0,RoleTemplateID=1,ScreenId=6},
                new AccessPage {PageDesc="Inquiry",CrudLevel=0,RoleTemplateID=2,ScreenId=6},
                new AccessPage {PageDesc="Inquiry",CrudLevel=2,RoleTemplateID=3,ScreenId=6},
                new AccessPage {PageDesc="Inquiry",CrudLevel=3,RoleTemplateID=4,ScreenId=6},
                new AccessPage {PageDesc="Inquiry",CrudLevel=3,RoleTemplateID=5,ScreenId=6},
                new AccessPage {PageDesc="Inquiry",CrudLevel=4,RoleTemplateID=6,ScreenId=6},
                new AccessPage {PageDesc="Inquiry",CrudLevel=4,RoleTemplateID=7,ScreenId=6},
                new AccessPage {PageDesc="Inquiry",CrudLevel=4,RoleTemplateID=8,ScreenId=6},

                new AccessPage {PageDesc="Reporting",CrudLevel=0,RoleTemplateID=1,ScreenId=7},
                new AccessPage {PageDesc="Reporting",CrudLevel=1,RoleTemplateID=2,ScreenId=7},
                new AccessPage {PageDesc="Reporting",CrudLevel=2,RoleTemplateID=3,ScreenId=7},
                new AccessPage {PageDesc="Reporting",CrudLevel=3,RoleTemplateID=4,ScreenId=7},
                new AccessPage {PageDesc="Reporting",CrudLevel=3,RoleTemplateID=5,ScreenId=7},
                new AccessPage {PageDesc="Reporting",CrudLevel=4,RoleTemplateID=6,ScreenId=7},
                new AccessPage {PageDesc="Reporting",CrudLevel=4,RoleTemplateID=7,ScreenId=7},
                new AccessPage {PageDesc="Reporting",CrudLevel=4,RoleTemplateID=8,ScreenId=7},

                new AccessPage {PageDesc="Individual",CrudLevel=0,RoleTemplateID=1,ScreenId=8},
                new AccessPage {PageDesc="Individual",CrudLevel=1,RoleTemplateID=2,ScreenId=8},
                new AccessPage {PageDesc="Individual",CrudLevel=2,RoleTemplateID=3,ScreenId=8},
                new AccessPage {PageDesc="Individual",CrudLevel=3,RoleTemplateID=4,ScreenId=8},
                new AccessPage {PageDesc="Individual",CrudLevel=3,RoleTemplateID=5,ScreenId=8},
                new AccessPage {PageDesc="Individual",CrudLevel=4,RoleTemplateID=6,ScreenId=8},
                new AccessPage {PageDesc="Individual",CrudLevel=4,RoleTemplateID=7,ScreenId=8},
                new AccessPage {PageDesc="Individual",CrudLevel=4,RoleTemplateID=8,ScreenId=8},

                new AccessPage {PageDesc="DeskOfficer",CrudLevel=0,RoleTemplateID=1,ScreenId=9},
                new AccessPage {PageDesc="DeskOfficer",CrudLevel=1,RoleTemplateID=2,ScreenId=9},
                new AccessPage {PageDesc="DeskOfficer",CrudLevel=0,RoleTemplateID=3,ScreenId=9},
                new AccessPage {PageDesc="DeskOfficer",CrudLevel=0,RoleTemplateID=4,ScreenId=9},
                new AccessPage {PageDesc="DeskOfficer",CrudLevel=0,RoleTemplateID=5,ScreenId=9},
                new AccessPage {PageDesc="DeskOfficer",CrudLevel=0,RoleTemplateID=6,ScreenId=9},
                new AccessPage {PageDesc="DeskOfficer",CrudLevel=0,RoleTemplateID=7,ScreenId=9},
                new AccessPage {PageDesc="DeskOfficer",CrudLevel=0,RoleTemplateID=8,ScreenId=9},

                new AccessPage {PageDesc="Profile",CrudLevel=0,RoleTemplateID=1,ScreenId=10},
                new AccessPage {PageDesc="Profile",CrudLevel=0,RoleTemplateID=2,ScreenId=10},
                new AccessPage {PageDesc="Profile",CrudLevel=2,RoleTemplateID=3,ScreenId=10},
                new AccessPage {PageDesc="Profile",CrudLevel=3,RoleTemplateID=4,ScreenId=10},
                new AccessPage {PageDesc="Profile",CrudLevel=3,RoleTemplateID=5,ScreenId=10},
                new AccessPage {PageDesc="Profile",CrudLevel=4,RoleTemplateID=6,ScreenId=10},
                new AccessPage {PageDesc="Profile",CrudLevel=4,RoleTemplateID=7,ScreenId=10},
                new AccessPage {PageDesc="Profile",CrudLevel=4,RoleTemplateID=8,ScreenId=10},

                new AccessPage {PageDesc="Process",CrudLevel=0,RoleTemplateID=1,ScreenId=11},
                new AccessPage {PageDesc="Process",CrudLevel=1,RoleTemplateID=2,ScreenId=11},
                new AccessPage {PageDesc="Process",CrudLevel=2,RoleTemplateID=3,ScreenId=11},
                new AccessPage {PageDesc="Process",CrudLevel=3,RoleTemplateID=4,ScreenId=11},
                new AccessPage {PageDesc="Process",CrudLevel=3,RoleTemplateID=5,ScreenId=11},
                new AccessPage {PageDesc="Process",CrudLevel=4,RoleTemplateID=6,ScreenId=11},
                new AccessPage {PageDesc="Process",CrudLevel=4,RoleTemplateID=7,ScreenId=11},
                new AccessPage {PageDesc="Process",CrudLevel=4,RoleTemplateID=8,ScreenId=11},
            };
            accesspages.ForEach(s => context.AccessPages.Add(s));
            context.SaveChanges();

            var users = new List<Users>
            {
                new Users {FullName="Abd Aziz Bin Hamzah",Username="aziz",Password="81dc9bdb52d04dc20036dbd8313ed055",Email="aziz.h@pl.gov.my", RoleTemplateID=6,DepartmentID=1},
                new Users {FullName="Soffiyan Bin Hadis",Username="soffiyan",Password="81dc9bdb52d04dc20036dbd8313ed055",Email="soffiyan.hadis@pl.gov.my", RoleTemplateID=4,DepartmentID=1},
                new Users {FullName="Hjh. Simai Binti Md Jamil",Username="simai",Password="81dc9bdb52d04dc20036dbd8313ed055",Email="simai@pl.gov.my", RoleTemplateID=3,DepartmentID=1},
                new Users {FullName="Suriani Salleh",Username="suriani",Password="81dc9bdb52d04dc20036dbd8313ed055",Email="suriani.salleh@pl.gov.my", RoleTemplateID=1,DepartmentID=1},
                new Users {FullName="Suwardi Binti Muali",Username="suwardi",Password="81dc9bdb52d04dc20036dbd8313ed055",Email="suwardi.muali.pl@1govuc.gov.my", RoleTemplateID=2,DepartmentID=1},
                new Users {FullName="Adey Suhaimi Bin Suhaili",Username="adey",Password="81dc9bdb52d04dc20036dbd8313ed055",Email="adey.suhaimi.pl@1govuc.gov.my", RoleTemplateID=3,DepartmentID=1},
                new Users {FullName="Azean Irdawati Binti Wahid",Username="azean",Password="81dc9bdb52d04dc20036dbd8313ed055",Email="azean.wahid.pl@1govuc.gov.my", RoleTemplateID=3,DepartmentID=1},
                new Users {FullName="Kazlina Binti Kassim",Username="kazlina",Password="81dc9bdb52d04dc20036dbd8313ed055",Email="kazlina@yahoo.com", RoleTemplateID=1,DepartmentID=1},
                new Users {FullName="Mat Daly Bin Matdin",Username="matdaly",Password="81dc9bdb52d04dc20036dbd8313ed055",Email="mat.daly@yahoo.com", RoleTemplateID=1,DepartmentID=1},
                new Users {FullName="Patimah Binti Hj. Lamat",Username="patimah",Password="81dc9bdb52d04dc20036dbd8313ed055",Email="patimah@yahoo.com", RoleTemplateID=1,DepartmentID=1},
                new Users {FullName="Rafidah Binti Mohd Isa",Username="rafidah",Password="81dc9bdb52d04dc20036dbd8313ed055",Email="rafidah@yahoo.com", RoleTemplateID=1,DepartmentID=1},
                new Users {FullName="Ahmad Jais Bin Halon",Username="ahmadjais",Password="81dc9bdb52d04dc20036dbd8313ed055",Email="ahmad.jais@yahoo.com", RoleTemplateID=1,DepartmentID=1},
                new Users {FullName="YBHG. Datuk Azhar Bin Ahmad",Username="kpe",Password="81dc9bdb52d04dc20036dbd8313ed055",Email="azharahmad@pl.gov.my", RoleTemplateID=7,DepartmentID=1},
                new Users {FullName="Mazalan Bin Hassin",Username="mazalan",Password="81dc9bdb52d04dc20036dbd8313ed055",Email="mazalan.hassin@pl.gov.my", RoleTemplateID=8,DepartmentID=1},
                new Users {FullName="R. Norasliana Binti Ramlee",Username="norasliana",Password="81dc9bdb52d04dc20036dbd8313ed055",Email="ana.ramli@pl.gov.my", RoleTemplateID=8,DepartmentID=1},
                new Users {FullName="Jabatan Bomba",Username="bomba",Password="81dc9bdb52d04dc20036dbd8313ed055",Email="jbpm_labuan.bomba@1govuc.gov.my", RoleTemplateID=5,DepartmentID=1},
                new Users {FullName="Ronny Jimmy",Username="ronny",Password="rGWQ/rZGq74=",Email="ronnyrtg@yahoo.com", RoleTemplateID=8,DepartmentID=1},
            };
            users.ForEach(s => context.Users.Add(s));
            context.SaveChanges();

        }
    }
}
