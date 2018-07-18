namespace TradingLicense.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using TradingLicense.Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<TradingLicense.Data.LicenseApplicationContext>
    {
        public Configuration()
        {
            AutomaticMigrationDataLossAllowed = true;
            AutomaticMigrationsEnabled = true;
            ContextKey = "TradingLicense.Data.LicenseApplicationContext";
        }

        protected override void Seed(TradingLicense.Data.LicenseApplicationContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var licType = new List<LIC_TYPE>
            {
                new LIC_TYPE {LIC_TYPECODE="TPP", LIC_TYPEDESC="Lesen Tred, Perniagaan & Perindustrian"},
                new LIC_TYPE {LIC_TYPECODE="PM", LIC_TYPEDESC="Lesen Petempatan Makanan"},
                new LIC_TYPE {LIC_TYPECODE="HRT", LIC_TYPEDESC="Lesen Hotel dan Rumah Tumpangan"},
                new LIC_TYPE {LIC_TYPECODE="PS", LIC_TYPEDESC="Lesen Pengurusan Skrap"},
                new LIC_TYPE {LIC_TYPECODE="IK", LIC_TYPEDESC="Lesen Iklan"},
                new LIC_TYPE {LIC_TYPECODE="PJ", LIC_TYPEDESC="Lesen Penjaja"},
                new LIC_TYPE {LIC_TYPECODE="PA", LIC_TYPEDESC="Lesen Pasar"},
                new LIC_TYPE {LIC_TYPECODE="MK", LIC_TYPEDESC="Lesen Minuman Keras"},
                new LIC_TYPE {LIC_TYPECODE="PPW", LIC_TYPEDESC="Lesen Pemberi Pinjam Wang"},
                new LIC_TYPE {LIC_TYPECODE="HI", LIC_TYPEDESC="Lesen Hiburan"},

            };
            licType.ForEach(s => context.LIC_TYPEs.Add(s));
            context.SaveChanges();

            var sector = new List<Sector>
            {
                new Sector {SectorID=1,SectorDesc="Tred"},
                new Sector {SectorID=2,SectorDesc="Stor"},
                new Sector {SectorID=3,SectorDesc="Perindustrian"},
                new Sector {SectorID=4,SectorDesc="Bengkel"},
                new Sector {SectorID=5,SectorDesc="Pertanian/Penternakan"},
                new Sector {SectorID=6,SectorDesc="Lain-lain"},
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
                new BusinessType {BusinessTypeCode="G",BusinessTypeDesc="JABATAN KERAJAAN" },
                new BusinessType {BusinessTypeCode="E",BusinessTypeDesc="SEKOLAH/INSTITUSI PENDIDIKAN" },
            };
            businesstypes.ForEach(s => context.BusinessTypes.Add(s));
            context.SaveChanges();

            var bts = new List<BT>
            {
                new BT {BT_CODE="I",BT_DESC="Hak Milik Perseorangan" },
                new BT {BT_CODE="O",BT_DESC="Lain-lain" },
                new BT {BT_CODE="P",BT_DESC="Perkongsian" },
                new BT {BT_CODE="U",BT_DESC="Syarikat Awam Berhad" },
                new BT {BT_CODE="C",BT_DESC="Syarikat Kerjasama" },
                new BT {BT_CODE="R",BT_DESC="Syarikat Sendirian Berhad" },
                new BT {BT_CODE="G",BT_DESC="Jabatan Kerajaan" },
                new BT {BT_CODE="E",BT_DESC="Sekolah/Institusi Pendidikan" },
            };
            bts.ForEach(s => context.BT.Add(s));
            context.SaveChanges();

            var race = new List<Race>
            {
                new Race {RaceDesc="Melayu"},
                new Race {RaceDesc="Cina"},
                new Race {RaceDesc="India"},
                new Race {RaceDesc="Bumiputra Sabah"},
                new Race {RaceDesc="Bumiputra Sarawak"},
                new Race {RaceDesc="Bangsa dari luar negara Malaysia"},
            };
            race.ForEach(s => context.Races.Add(s));
            context.SaveChanges();

            var entmtGroup = new List<E_GROUP>
            {
                new E_GROUP {E_GROUPID=1,E_G_CODE="L001",E_G_DESC="Oditorium/Dewan"},
                new E_GROUP {E_GROUPID=2,E_G_CODE="L002",E_G_DESC="Panggung Wayang/Panggung"},
                new E_GROUP {E_GROUPID=3,E_G_CODE="L003",E_G_DESC="Pusat Hiburan (Dalam Bangunan)"},
                new E_GROUP {E_GROUPID=4,E_G_CODE="L004",E_G_DESC="Pusat Hiburan/Taman Hiburan (Luar Bangunan)"},
                new E_GROUP {E_GROUPID=5,E_G_CODE="L005",E_G_DESC="Lorong Boling"},
                new E_GROUP {E_GROUPID=6,E_G_CODE="L006",E_G_DESC="Gelanggang Luncur"},
                new E_GROUP {E_GROUPID=7,E_G_CODE="L007",E_G_DESC="Mesin Hiburan (Kiddy Rides/Juke Box) di tempat selain daripada Pusat Hiburan"},
                new E_GROUP {E_GROUPID=8,E_G_CODE="L008",E_G_DESC="Dewan Tarian/Disko/Kabaret"},
                new E_GROUP {E_GROUPID=9,E_G_CODE="L009",E_G_DESC="Dewan Biliard/Snuker"},
                new E_GROUP {E_GROUPID=10,E_G_CODE="L010",E_G_DESC="Stadium"},
                new E_GROUP {E_GROUPID=11,E_G_CODE="L011",E_G_DESC="Ruang Legar/Dewan/Tempat Terbuka yang digunakan bagi pameran"},
            };
            entmtGroup.ForEach(s => context.E_GROUPs.Add(s));
            context.SaveChanges();

            var businesscode = new List<BusinessCode>
            {
                new BusinessCode {CodeNumber="A001",SectorID=1,CodeDesc="Pejabat urusan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A002",SectorID=1,CodeDesc="Kemudahan dan perkhidmatan jagaan kesihatan swasta",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A003",SectorID=1,CodeDesc="Bank dan institusi kewangan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A004",SectorID=1,CodeDesc="Kedai buku dan alat tulis",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A005",SectorID=1,CodeDesc="Institusi pendidikan tinggi swasta, sekolah swasta atau institusi pendidikan swasta",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A006",SectorID=1,CodeDesc="Barangan elektrik atau elektronik atau komputer",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A007",SectorID=1,CodeDesc="Perabot",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A008",SectorID=1,CodeDesc="Hiasan dalaman/barangan hiasan dalaman",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A009",SectorID=1,CodeDesc="Peralatan dan perkakasan rumah/pejabat",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A010",SectorID=1,CodeDesc="Peralatan dan kelengkapan dapur",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A011",SectorID=1,CodeDesc="Perkhidmatan menjahit",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A012",SectorID=1,CodeDesc="Pakaian, tekstil dan alat jahitan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A013",SectorID=1,CodeDesc="Kosmetik dan kelengkapan dandanan diri",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A014",SectorID=1,CodeDesc="Peralatan makmal, saintifik dan perubatan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A015",SectorID=1,CodeDesc="Ubat, farmasi dan produk kesihatan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A016",SectorID=1,CodeDesc="Barang kemas, hiasan diri dan persendirian",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A017",SectorID=1,CodeDesc="Jam",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A018",SectorID=1,CodeDesc="Bagasi",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A019",SectorID=1,CodeDesc="Cenderamata",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A020",SectorID=1,CodeDesc="Barangan optik",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A021",SectorID=1,CodeDesc="Peralatan, aksesori dan perkhidmatan fotografi",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A022",SectorID=1,CodeDesc="Bunga dan tumbuhan tiruan/hidup",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A023",SectorID=1,CodeDesc="Perkhidmatan andaman dan pakaian pengantin",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A024",SectorID=1,CodeDesc="Telefon dan aksesori",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A025",SectorID=1,CodeDesc="Barangan antik",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A026",SectorID=1,CodeDesc="Menjilid buku atau membuat fotokopi",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A027",SectorID=1,CodeDesc="Tukang kunci",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A028",SectorID=1,CodeDesc="Pusat/studio rakaman audio",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A029",SectorID=1,CodeDesc="Stesen minyak/gas/elektrik",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A030",SectorID=1,CodeDesc="Media digital/elektronik dan aksesori berkaitan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A031",SectorID=1,CodeDesc="Barangan logam (untuk sektor pembinaan dan pembuatan)",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A032",SectorID=1,CodeDesc="Alat-alat muzik atau kelengkapan muzik",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A033",SectorID=1,CodeDesc="Kelas kesenian/kebudayaan/kemahiran",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A034",SectorID=1,CodeDesc="Peralatan, bahan dan hiasan landskap",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A035",SectorID=1,CodeDesc="Kenderaan berat",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A036",SectorID=1,CodeDesc="Kereta, motosikal, bot dan jet ski",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A037",SectorID=1,CodeDesc="Mesin dan jentera",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A038",SectorID=1,CodeDesc="Basikal dan aksesori",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A039",SectorID=1,CodeDesc="Peralatan kesihatan, kecergasan dan sukan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A040",SectorID=1,CodeDesc="Produk berasaskan tembakau (seperti rokok/curut dan produk seumpamanya)",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A041",SectorID=1,CodeDesc="Kemasan bangunan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A042",SectorID=1,CodeDesc="Alat permainan dan barangan hobi",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A043",SectorID=1,CodeDesc="Percetakan digital",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A044",SectorID=1,CodeDesc="Kedai barangan runcit/kedai serbaneka (seperti pasar raya dan gedung)",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A045",SectorID=1,CodeDesc="Sewaan kereta",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A046",SectorID=1,CodeDesc="Haiwan peliharaan, dandanan binatang, peralatan dan makanan haiwan dan/atau rumah tumpangan haiwan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A047",SectorID=1,CodeDesc="Agensi teman sosial",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A048",SectorID=1,CodeDesc="Kedai bebas cukai",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A049",SectorID=1,CodeDesc="Pengurusan mayat dan pengebumian",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A050",SectorID=1,CodeDesc="Baja, racun atau kimia-kimia lain yang serupa",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A051",SectorID=1,CodeDesc="Bahan berbahaya yang mudah terbakar tidak termasuk petroleum dan gas",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A052",SectorID=1,CodeDesc="Dobi",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A053",SectorID=1,CodeDesc="Stor kayu",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A054",SectorID=1,CodeDesc="Agensi pelancongan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A055",SectorID=1,CodeDesc="Agensi pekerjaan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A056",SectorID=1,CodeDesc="Jualan tiket pengangkutan awam",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A057",SectorID=1,CodeDesc="Pusat kecantikan dan penjagaan kesihatan",DefaultRate=25f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A058",SectorID=1,CodeDesc="Joki kereta",DefaultRate=0.0f,BaseFee=100.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A059",SectorID=1,CodeDesc="Pemberi Pinjam Wang",DefaultRate=0.0f,BaseFee=0.0f,Period=1,PeriodQuantity=2},
                new BusinessCode {CodeNumber="B001",SectorID=2,CodeDesc="Gudang/stor makanan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="B002",SectorID=2,CodeDesc="Gudang/stor barang-barang lain",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="B003",SectorID=2,CodeDesc="Gudang/stor bahan merbahaya",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C001",SectorID=3,CodeDesc="Barang-barang berasaskan logam",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C002",SectorID=3,CodeDesc="Media digital dan elektronik",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C003",SectorID=3,CodeDesc="Makanan dan perkakasan haiwan",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C004",SectorID=3,CodeDesc="Baja",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C005",SectorID=3,CodeDesc="Kenderaan, jentera dan mesin",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C006",SectorID=3,CodeDesc="Alat-alat ganti dan aksesori kenderaan bermotor",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C007",SectorID=3,CodeDesc="Basikal",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C008",SectorID=3,CodeDesc="Tayar dan tiub",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C009",SectorID=3,CodeDesc="Bahan binaan",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C010",SectorID=3,CodeDesc="Bateri",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C011",SectorID=3,CodeDesc="Kabel dan wayar",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C012",SectorID=3,CodeDesc="Permaidani dan hamparan",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C013",SectorID=3,CodeDesc="Keranda dan batu nisan",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C014",SectorID=3,CodeDesc="Kosmetik dan kelengkapan dandanan diri",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C015",SectorID=3,CodeDesc="Bahan pencuci, alat-alat mencuci bahan-bahan lain yang seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C016",SectorID=3,CodeDesc="Bahan pengilap",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C017",SectorID=3,CodeDesc="Mengisi gas ke dalam botol atau silinder",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C018",SectorID=3,CodeDesc="Dadah/ubat-ubatan dan keluaran-keluaran farmasi",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C019",SectorID=3,CodeDesc="Fabrik/kulit atau pakaian",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C020",SectorID=3,CodeDesc="Menjahit dan menyulam",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C021",SectorID=3,CodeDesc="Barang-barang elektrik dan elektronik",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C022",SectorID=2,CodeDesc="Barang-barang komputer",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C023",SectorID=2,CodeDesc="Membuat barang-barang perubatan, saintifik dan makmal",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C024",SectorID=2,CodeDesc="Kaca dan cermin",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C025",SectorID=2,CodeDesc="Anggota badan palsu",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C026",SectorID=2,CodeDesc="Produk dibuat daripada gelas serabut, gentian sintetik, kapas tali dan produk seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C027",SectorID=2,CodeDesc="Mercun dan bahan letupan",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C028",SectorID=2,CodeDesc="Gas mudah terbakar",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C029",SectorID=3,CodeDesc="Keluaran petroleum termasuk minyak pelincir dan lain-lain minyak yang seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C030",SectorID=3,CodeDesc="Produk berasaskan emas, perak, tembaga dan bahan-bahan yang seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C031",SectorID=3,CodeDesc="Peralatan sembahyang dan barang-barang lain yang berkaitan",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C032",SectorID=3,CodeDesc="Batu kapur",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C033",SectorID=3,CodeDesc="Cat",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C034",SectorID=3,CodeDesc="Kertas dan hasil-hasil kertas",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C035",SectorID=3,CodeDesc="Plastik, barang-barang daripada bahan plastik atau bahan lain yang seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C036",SectorID=3,CodeDesc="Kaca, barang-barang daripada bahan kaca atau bahan lain yang seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C037",SectorID=3,CodeDesc="Logam, barang-barang daripada bahan logam atau bahan lain yang seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C038",SectorID=3,CodeDesc="Papan dan kayu",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C039",SectorID=3,CodeDesc="Perabot",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C040",SectorID=3,CodeDesc="Barang-barang tembikar",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C041",SectorID=3,CodeDesc="Percetakan (berskala besar)",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C042",SectorID=3,CodeDesc="Pam dan penapis air",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C043",SectorID=3,CodeDesc="Keluaran getah",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C044",SectorID=3,CodeDesc="Tirai dan bidai",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C045",SectorID=3,CodeDesc="Alat tulis/buku/majalah",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C046",SectorID=3,CodeDesc="Barang-barang mainan",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C047",SectorID=3,CodeDesc="Barangan pertanian dan kimia industri",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C048",SectorID=3,CodeDesc="Blok ais",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C049",SectorID=3,CodeDesc="Visual iklan",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C050",SectorID=3,CodeDesc="Makanan dan seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C051",SectorID=3,CodeDesc="Minuman dan seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="D001",SectorID=4,CodeDesc="Alat ganti dan aksesori",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="D002",SectorID=4,CodeDesc="Pemasangan penyaman udara di kenderaan",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="D003",SectorID=4,CodeDesc="Kenderaan bermotor dan kenderaan marin",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="D004",SectorID=4,CodeDesc="Tayar",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="D005",SectorID=4,CodeDesc="Mencuci dan/atau mengilap kereta",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="D006",SectorID=4,CodeDesc="Menyembur cat, selulosa dan bahan-bahan kimia lain",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="D007",SectorID=4,CodeDesc="Kerja-kerja kejuruteraan",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="D008",SectorID=4,CodeDesc="Kerja-kerja kimpalan",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="D009",SectorID=4,CodeDesc="Pertukangan batu, kayu, kaca dan logam (termasuk papan iklan) ",DefaultRate=2.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="E001",SectorID=5,CodeDesc="Menternak burung walit",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="E002",SectorID=5,CodeDesc="Menternak lebah, lintah, cacing dan seumpamanya",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="E003",SectorID=5,CodeDesc="Tempat pembiakan haiwan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="E004",SectorID=5,CodeDesc="Rumah sembelihan binatang",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="E005",SectorID=5,CodeDesc="Kolam pancing",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="E006",SectorID=5,CodeDesc="Semaian tumbuhan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="F001",SectorID=6,CodeDesc="Mana-mana aktiviti perniagaan yang tidak termasuk dalam Jadual",DefaultRate=1.5f,BaseFee=0.0f,Period=1,PeriodQuantity=1},
            };
            businesscode.ForEach(s => context.BusinessCodes.Add(s));
            context.SaveChanges();

            var bcode = new List<BC>
            {
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A01",C_R_DESC="Pejabat urusan",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A02",C_R_DESC="Kemudahan dan perkhidmatan jagaan kesihatan swasta",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A03",C_R_DESC="Bank dan institusi kewangan",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A04",C_R_DESC="Kedai buku dan alat tulis",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A05",C_R_DESC="Institusi pendidikan tinggi swasta, sekolah swasta atau institusi pendidikan swasta",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A06",C_R_DESC="Barangan elektrik atau elektronik atau komputer",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A07",C_R_DESC="Perabot",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A08",C_R_DESC="Hiasan dalaman/barangan hiasan dalaman",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A09",C_R_DESC="Peralatan dan perkakasan rumah/pejabat",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A10",C_R_DESC="Peralatan dan kelengkapan dapur",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A11",C_R_DESC="Perkhidmatan menjahit",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A12",C_R_DESC="Pakaian, tekstil dan alat jahitan",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A13",C_R_DESC="Kosmetik dan kelengkapan dandanan diri",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A14",C_R_DESC="Peralatan makmal, saintifik dan perubatan",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A15",C_R_DESC="Ubat, farmasi dan produk kesihatan",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A16",C_R_DESC="Barang kemas, hiasan diri dan persendirian",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A17",C_R_DESC="Jam",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A18",C_R_DESC="Bagasi",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A19",C_R_DESC="Cenderamata",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A20",C_R_DESC="Barangan optik",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A21",C_R_DESC="Peralatan, aksesori dan perkhidmatan fotografi",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A22",C_R_DESC="Bunga dan tumbuhan tiruan/hidup",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A23",C_R_DESC="Perkhidmatan andaman dan pakaian pengantin",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A24",C_R_DESC="Telefon dan aksesori",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A25",C_R_DESC="Barangan antik",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A26",C_R_DESC="Menjilid buku atau membuat fotokopi",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A27",C_R_DESC="Tukang kunci",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A28",C_R_DESC="Pusat/studio rakaman audio",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A29",C_R_DESC="Stesen minyak/gas/elektrik",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A30",C_R_DESC="Media digital/elektronik dan aksesori berkaitan",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A31",C_R_DESC="Barangan logam (untuk sektor pembinaan dan pembuatan)",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A32",C_R_DESC="Alat-alat muzik atau kelengkapan muzik",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A33",C_R_DESC="Kelas kesenian/kebudayaan/kemahiran",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A34",C_R_DESC="Peralatan, bahan dan hiasan landskap",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A35",C_R_DESC="Kenderaan berat",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A36",C_R_DESC="Kereta, motosikal, bot dan jet ski",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A37",C_R_DESC="Mesin dan jentera",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A38",C_R_DESC="Basikal dan aksesori",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A39",C_R_DESC="Peralatan kesihatan, kecergasan dan sukan",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A40",C_R_DESC="Produk berasaskan tembakau (seperti rokok/curut dan produk seumpamanya)",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A41",C_R_DESC="Kemasan bangunan",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A42",C_R_DESC="Alat permainan dan barangan hobi",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A43",C_R_DESC="Percetakan digital",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A44",C_R_DESC="Kedai barangan runcit/kedai serbaneka (seperti pasar raya dan gedung)",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A45",C_R_DESC="Sewaan kereta",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A46",C_R_DESC="Haiwan peliharaan, dandanan binatang, peralatan dan makanan haiwan dan/atau rumah tumpangan haiwan",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A47",C_R_DESC="Agensi teman sosial",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A48",C_R_DESC="Kedai bebas cukai",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A49",C_R_DESC="Pengurusan mayat dan pengebumian",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A50",C_R_DESC="Baja, racun atau kimia-kimia lain yang serupa",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A51",C_R_DESC="Bahan berbahaya yang mudah terbakar tidak termasuk petroleum dan gas",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A52",C_R_DESC="Dobi",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A53",C_R_DESC="Stor kayu",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A54",C_R_DESC="Agensi pelancongan",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A55",C_R_DESC="Agensi pekerjaan",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A56",C_R_DESC="Jualan tiket pengangkutan awam",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A57",C_R_DESC="Pusat kecantikan dan penjagaan kesihatan",DEF_RATE=25f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A58",C_R_DESC="Joki kereta",DEF_RATE=0.0f,BASE_FEE=1.0f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=1,C_R="A59",C_R_DESC="Pemberi Pinjam Wang",DEF_RATE=0.0f,PERIOD=1,PERIOD_Q=2},
                new BC {LIC_TYPEID=1,SECTORID=2,C_R="B01",C_R_DESC="Gudang/stor makanan",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=2,C_R="B02",C_R_DESC="Gudang/stor barang-barang lain",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=2,C_R="B03",C_R_DESC="Gudang/stor bahan merbahaya",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C01",C_R_DESC="Barang-barang berasaskan logam",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C02",C_R_DESC="Media digital dan elektronik",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C03",C_R_DESC="Makanan dan perkakasan haiwan",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C04",C_R_DESC="Baja",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C05",C_R_DESC="Kenderaan, jentera dan mesin",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C06",C_R_DESC="Alat-alat ganti dan aksesori kenderaan bermotor",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C07",C_R_DESC="Basikal",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C08",C_R_DESC="Tayar dan tiub",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C09",C_R_DESC="Bahan binaan",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C10",C_R_DESC="Bateri",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C11",C_R_DESC="Kabel dan wayar",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C12",C_R_DESC="Permaidani dan hamparan",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C13",C_R_DESC="Keranda dan batu nisan",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C14",C_R_DESC="Kosmetik dan kelengkapan dandanan diri",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C15",C_R_DESC="Bahan pencuci, alat-alat mencuci bahan-bahan lain yang seumpamanya",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C16",C_R_DESC="Bahan pengilap",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C17",C_R_DESC="Mengisi gas ke dalam botol atau silinder",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C18",C_R_DESC="Dadah/ubat-ubatan dan keluaran-keluaran farmasi",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C19",C_R_DESC="Fabrik/kulit atau pakaian",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C20",C_R_DESC="Menjahit dan menyulam",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C21",C_R_DESC="Barang-barang elektrik dan elektronik",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=2,C_R="B01",C_R_DESC="Barang-barang komputer",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=2,C_R="B02",C_R_DESC="Membuat barang-barang perubatan, saintifik dan makmal",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=2,C_R="B03",C_R_DESC="Kaca dan cermin",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=2,C_R="B04",C_R_DESC="Anggota badan palsu",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=2,C_R="B05",C_R_DESC="Produk dibuat daripada gelas serabut, gentian sintetik, kapas tali dan produk seumpamanya",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=2,C_R="B06",C_R_DESC="Mercun dan bahan letupan",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=2,C_R="B07",C_R_DESC="Gas mudah terbakar",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C01",C_R_DESC="Keluaran petroleum termasuk minyak pelincir dan lain-lain minyak yang seumpamanya",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C02",C_R_DESC="Produk berasaskan emas, perak, tembaga dan bahan-bahan yang seumpamanya",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C03",C_R_DESC="Peralatan sembahyang dan barang-barang lain yang berkaitan",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C04",C_R_DESC="Batu kapur",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C05",C_R_DESC="Cat",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C06",C_R_DESC="Kertas dan hasil-hasil kertas",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C07",C_R_DESC="Plastik, barang-barang daripada bahan plastik atau bahan lain yang seumpamanya",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C08",C_R_DESC="Kaca, barang-barang daripada bahan kaca atau bahan lain yang seumpamanya",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C09",C_R_DESC="Logam, barang-barang daripada bahan logam atau bahan lain yang seumpamanya",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C10",C_R_DESC="Papan dan kayu",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C11",C_R_DESC="Perabot",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C12",C_R_DESC="Barang-barang tembikar",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C13",C_R_DESC="Percetakan (berskala besar)",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C14",C_R_DESC="Pam dan penapis air",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C15",C_R_DESC="Keluaran getah",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C16",C_R_DESC="Tirai dan bidai",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C17",C_R_DESC="Alat tulis/buku/majalah",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C18",C_R_DESC="Barang-barang mainan",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C19",C_R_DESC="Barangan pertanian dan kimia industri",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C20",C_R_DESC="Blok ais",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C21",C_R_DESC="Visual iklan",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C22",C_R_DESC="Makanan dan seumpamanya",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=3,C_R="C23",C_R_DESC="Minuman dan seumpamanya",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=4,C_R="D01",C_R_DESC="Alat ganti dan aksesori",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=4,C_R="D02",C_R_DESC="Pemasangan penyaman udara di kenderaan",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=4,C_R="D03",C_R_DESC="Kenderaan bermotor dan kenderaan marin",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=4,C_R="D04",C_R_DESC="Tayar",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=4,C_R="D05",C_R_DESC="Mencuci dan/atau mengilap kereta",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=4,C_R="D06",C_R_DESC="Menyembur cat, selulosa dan bahan-bahan kimia lain",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=4,C_R="D07",C_R_DESC="Kerja-kerja kejuruteraan",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=4,C_R="D08",C_R_DESC="Kerja-kerja kimpalan",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=4,C_R="D09",C_R_DESC="Pertukangan batu, kayu, kaca dan logam (termasuk papan iklan) ",DEF_RATE=2.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=5,C_R="E01",C_R_DESC="Menternak burung walit",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=5,C_R="E02",C_R_DESC="Menternak lebah, lintah, cacing dan seumpamanya",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=5,C_R="E03",C_R_DESC="Tempat pembiakan haiwan",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=5,C_R="E04",C_R_DESC="Rumah sembelihan binatang",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=5,C_R="E05",C_R_DESC="Kolam pancing",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=5,C_R="E06",C_R_DESC="Semaian tumbuhan",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=1,SECTORID=6,C_R="F07",C_R_DESC="Mana-mana aktiviti perniagaan yang tidak termasuk dalam Jadual",DEF_RATE=1.5f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=2,C_R="G01",C_R_DESC="Restoran/kedai makan/gerai makan/kios makanan",DEF_RATE=10.0f,BASE_FEE=0.0f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=2,C_R="G02",C_R_DESC="Menjual makanan/minuman (tanpa tempat makan)",DEF_RATE=10.0f,BASE_FEE=0.0f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=2,C_R="G03",C_R_DESC="Katering makanan",DEF_RATE=10.0f,BASE_FEE=0.0f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=2,C_R="G04",C_R_DESC="Kantin sekolah",DEF_RATE=10.0f,BASE_FEE=0.0f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=2,C_R="G05",C_R_DESC="Kantin pejabat",DEF_RATE=10.0f,BASE_FEE=0.0f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=3,C_R="H01",C_R_DESC="Hotel (Kelas Pertama)",DEF_RATE=0.0f,BASE_FEE=150.0f,PERIOD=2,PERIOD_Q=1},
                new BC {LIC_TYPEID=3,C_R="H02",C_R_DESC="Hotel (Kelas Kedua)",DEF_RATE=0.0f,BASE_FEE=1.0f,PERIOD=2,PERIOD_Q=1},
                new BC {LIC_TYPEID=3,C_R="H03",C_R_DESC="Hotel (Kelas Ketiga)",DEF_RATE=0.0f,BASE_FEE=30.0f,PERIOD=2,PERIOD_Q=1},
                new BC {LIC_TYPEID=3,C_R="H04",C_R_DESC="Lodging House/Rumah Tumpangan",DEF_RATE=0.0f,BASE_FEE=30.0f,PERIOD=2,PERIOD_Q=1},
                new BC {LIC_TYPEID=4,C_R="I01",C_R_DESC="Pengurusan Skrap/Dealing in Scrap",DEF_RATE=0.0f,BASE_FEE=25.0f,PERIOD=2,PERIOD_Q=3},
                new BC {LIC_TYPEID=5,C_R="J01",C_R_DESC="Iklan Tidak Bercahaya",EX_FEE=25.0f,PERIOD=1,PERIOD_Q=1,P_FEE=50.0f},
                new BC {LIC_TYPEID=5,C_R="J02",C_R_DESC="Iklan Bercahaya",EX_FEE=25.0f,PERIOD=1,PERIOD_Q=1,P_FEE=1.0f},
                new BC {LIC_TYPEID=5,C_R="J03",C_R_DESC="Iklan Kecil",EX_FEE=25.0f,PERIOD=1,PERIOD_Q=1,P_FEE=50.0f},
                new BC {LIC_TYPEID=5,C_R="J04",C_R_DESC="Iklan yang mengunjur lebih daripada 15 sentimeter melebihi bangunan – tidak bercahaya",EX_FEE=25.0f,PERIOD=1,PERIOD_Q=1,P_FEE=1.0f},
                new BC {LIC_TYPEID=5,C_R="J05",C_R_DESC="Iklan yang mengunjur lebih daripada 15 sentimeter melebihi bangunan – bercahaya",EX_FEE=25.0f,PERIOD=1,PERIOD_Q=1,P_FEE=2.0f},
                new BC {LIC_TYPEID=5,C_R="J06",C_R_DESC="Tanda Langit",PERIOD=1,PERIOD_Q=1,P_FEE=1.0f},
                new BC {LIC_TYPEID=6,C_R="K01",C_R_DESC="Penjaja Bergerak",P_FEE=48.0f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=6,C_R="K02",C_R_DESC="Penjaja Statik: Makanan",P_FEE=10.0f,PERIOD=2,PERIOD_Q=1},
                new BC {LIC_TYPEID=6,C_R="K03",C_R_DESC="Penjaja Statik: Selain daripada makanan",P_FEE=15.0f,PERIOD=2,PERIOD_Q=1},
                new BC {LIC_TYPEID=6,C_R="K04",C_R_DESC="Penjaja Sementara Bulanan",P_FEE=15.0f,PERIOD=2,PERIOD_Q=1},
                new BC {LIC_TYPEID=6,C_R="K05",C_R_DESC="Penjaja Sementara Harian",P_FEE=0.5f,PERIOD=4,PERIOD_Q=1},
                new BC {LIC_TYPEID=7,C_R="L01",C_R_DESC="Pasar Malam",P_FEE=1.0f,PERIOD=4,PERIOD_Q=1},
                new BC {LIC_TYPEID=7,C_R="L02",C_R_DESC="Ikan",P_FEE=30.0f,PERIOD=2,PERIOD_Q=1},
                new BC {LIC_TYPEID=7,C_R="L03",C_R_DESC="Ayam",P_FEE=30.0f,PERIOD=2,PERIOD_Q=1},
                new BC {LIC_TYPEID=7,C_R="L04",C_R_DESC="Daging",P_FEE=30.0f,PERIOD=2,PERIOD_Q=1},
                new BC {LIC_TYPEID=7,C_R="L05",C_R_DESC="Sayur-sayuran/buah-buahan/telur",P_FEE=20.0f,PERIOD=2,PERIOD_Q=1},
                new BC {LIC_TYPEID=7,C_R="L06",C_R_DESC="Barang-barang lain",P_FEE=20.0f,PERIOD=2,PERIOD_Q=1},
                new BC {LIC_TYPEID=7,C_R="L07",C_R_DESC="Pasar Persendirian - Luas lantai 70 meter persegi atau lebih",P_FEE=225.0f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=7,C_R="L08",C_R_DESC="Pasar Persendirian - Luas lantai kurang daripada 70 meter persegi",P_FEE=175.0f,PERIOD=1,PERIOD_Q=1},
                new BC {LIC_TYPEID=8,C_R="M01",C_R_DESC="Runcit",DEF_HOUR="7.pg. hingga 9.mlm",EX_HOUR_FEE=0.0f,PERIOD=2,PERIOD_Q=1,P_FEE=35.0f},
                new BC {LIC_TYPEID=8,C_R="M02",C_R_DESC="Borong",DEF_HOUR="7.pg. hingga 9.mlm",EX_HOUR_FEE=0.0f,PERIOD=2,PERIOD_Q=1,P_FEE=45.0f},
                new BC {LIC_TYPEID=8,C_R="M03",C_R_DESC="Rumah Awam (Kelas 1)",DEF_HOUR="10.pg. hingga 12.mlm",EX_HOUR_FEE=30.0f,PERIOD=2,PERIOD_Q=1,P_FEE=110.0f},
                new BC {LIC_TYPEID=8,C_R="M04",C_R_DESC="Rumah Awam (Kelas 2)",DEF_HOUR="10.pg. hingga 10.mlm",EX_HOUR_FEE=30.0f,PERIOD=2,PERIOD_Q=1,P_FEE=80.0f},
                new BC {LIC_TYPEID=8,C_R="M05",C_R_DESC="Rumah Awam (Kelas 3)",DEF_HOUR="10.pg. hingga 9.mlm",EX_HOUR_FEE=30.0f,PERIOD=2,PERIOD_Q=1,P_FEE=55.0f},
                new BC {LIC_TYPEID=8,C_R="M06",C_R_DESC="Rumah Bir (Kelas 1)",DEF_HOUR="10.pg. hingga 12.mlm",EX_HOUR_FEE=15.0f,PERIOD=2,PERIOD_Q=1,P_FEE=55.0f},
                new BC {LIC_TYPEID=8,C_R="M07",C_R_DESC="Rumah Bir (Kelas 2)",DEF_HOUR="10.pg. hingga 10.mlm",EX_HOUR_FEE=15.0f,PERIOD=2,PERIOD_Q=1,P_FEE=35.0f},
                new BC {LIC_TYPEID=8,C_R="M08",C_R_DESC="Lesen Sementara",DEF_HOUR="Tidak Berkenaan",EX_HOUR_FEE=0.0f,PERIOD=4,PERIOD_Q=1,P_FEE=30.0f},
                new BC {LIC_TYPEID=8,C_R="M09",C_R_DESC="Lesen Kedai Todi (Estet/Swasta)",DEF_HOUR="Tidak Berkenaan",EX_HOUR_FEE=0.0f,PERIOD=2,PERIOD_Q=1,P_FEE=4.5f},
                new BC {LIC_TYPEID=8,C_R="M10",C_R_DESC="Lesen Sadapan Todi",DEF_HOUR="Tidak Berkenaan",EX_HOUR_FEE=0.0f,PERIOD=2,PERIOD_Q=1,P_FEE=15.0f},
                new BC {LIC_TYPEID=9,C_R="N01",C_R_DESC="Lesen Pemberi Pinjam Wang",DEF_HOUR="Tidak Berkenaan",EX_HOUR_FEE=0.0f,PERIOD=1,PERIOD_Q=2,P_FEE=2000.0f},
                new BC {LIC_TYPEID=10,C_R="P01",C_R_DESC="Billiard/Snuker",O_FEE=10.0f,O_NAME="meja",PERIOD=2,PERIOD_Q=1},
                new BC {LIC_TYPEID=10,C_R="P02",C_R_DESC="Boling",O_FEE=5.0f,O_NAME="lorong",PERIOD=2,PERIOD_Q=1},
                new BC {LIC_TYPEID=10,C_R="P03",C_R_DESC="Pameran Filem Sinematograf - Dalam penggung/panggung wayang",O_FEE=5.0f,O_NAME="pertunjukan",PERIOD=0,PERIOD_Q=0},
                new BC {LIC_TYPEID=10,C_R="P04",C_R_DESC="Pameran Filem Sinematograf - Di tempat terbuka",O_FEE=5.0f,PERIOD=4,PERIOD_Q=1},
                new BC {LIC_TYPEID=10,C_R="P05",C_R_DESC="Sarkas",BASE_FEE=5.0f,O_FEE=2.5f,O_NAME="pertunjukan",PERIOD=0,PERIOD_Q=0},
                new BC {LIC_TYPEID=10,C_R="P06",C_R_DESC="Hiburan dengan Mesin Hiburan - Kiddy Rides",O_FEE=2.0f,O_NAME="mesin",PERIOD=2,PERIOD_Q=1},
                new BC {LIC_TYPEID=10,C_R="P07",C_R_DESC="Hiburan dengan Mesin Hiburan - Mesin video",O_FEE=5.0f,O_NAME="mesin",PERIOD=2,PERIOD_Q=1},
                new BC {LIC_TYPEID=10,C_R="P08",C_R_DESC="Hiburan di Pub/Coffee House/Lounge/Disko/Dewan Tarian - Muzik dan nyanyian",O_FEE=4.0f,O_NAME="sehingga 12 malam",PERIOD=4,PERIOD_Q=1},
                new BC {LIC_TYPEID=10,C_R="P09",C_R_DESC="Hiburan di Pub/Coffee House/Lounge/Disko/Dewan Tarian - Muzik dan nyanyian",O_FEE=8.0f,O_NAME="selepas 12 malam",PERIOD=4,PERIOD_Q=1},
                new BC {LIC_TYPEID=10,C_R="P10",C_R_DESC="Hiburan di Pub/Coffee House/Lounge/Disko/Dewan Tarian - Tarian",O_FEE=4.0f,O_NAME="sehingga 12 malam",PERIOD=4,PERIOD_Q=1},
                new BC {LIC_TYPEID=10,C_R="P11",C_R_DESC="Hiburan di Pub/Coffee House/Lounge/Disko/Dewan Tarian - Tarian",O_FEE=14.0f,O_NAME="selepas 12 malam",PERIOD=4,PERIOD_Q=1},
                new BC {LIC_TYPEID=10,C_R="P12",C_R_DESC="Pameran",O_FEE=5.0f,O_NAME="gerai",PERIOD=4,PERIOD_Q=1},
                new BC {LIC_TYPEID=10,C_R="P13",C_R_DESC="Pertunjukan Fesyen oleh Artis Profesional/Pertandingan Ratu Cantik",O_FEE=10.0f,O_NAME="artis/peserta",PERIOD=4,PERIOD_Q=1},
                new BC {LIC_TYPEID=10,C_R="P14",C_R_DESC="Pesta ria",O_FEE=2.0f,O_NAME="gerai",PERIOD=4,PERIOD_Q=1},
                new BC {LIC_TYPEID=10,C_R="P15",C_R_DESC="Hiburan Juke Box",O_FEE=10.0f,O_NAME="mesin",PERIOD=2,PERIOD_Q=1},
                new BC {LIC_TYPEID=10,C_R="P16",C_R_DESC="Pertunjukan Patung",O_FEE=5.0f,PERIOD=4,PERIOD_Q=1},
                new BC {LIC_TYPEID=10,C_R="P17",C_R_DESC="Apa-apa hiburan di luar kuil",O_FEE=5.0f,PERIOD=4,PERIOD_Q=1},
                new BC {LIC_TYPEID=10,C_R="P18",C_R_DESC="Apa-apa hiburan lain",O_FEE=10.0f,PERIOD=4,PERIOD_Q=1},
            };
            bcode.ForEach(s => context.BCs.Add(s));
            context.SaveChanges();

            var departments = new List<Department>
            {
                new Department {DepartmentCode="Pelesenan",DepartmentDesc="Bahagian Pelesenan",Internal=1, Routeable=0},
                new Department {DepartmentCode="ICT",DepartmentDesc="Jabatan Pengurusan Maklumat",Internal=1, Routeable=0},
                new Department {DepartmentCode="BPP",DepartmentDesc="Jabatan Perancangan & Kawalan Bangunan",Internal=1, Routeable=1},
                new Department {DepartmentCode="JPPPH",DepartmentDesc="Jabatan Penilaian, Pelaburan dan Pengurusan Harta",Internal=1, Routeable=1},
                new Department {DepartmentCode="UKS",DepartmentDesc="Unit Kesihatan",Internal=1, Routeable=1},
                new Department {DepartmentCode="PKPE",DepartmentDesc="Pejabat Ketua Pegawai Eksekutif",Internal=1, Routeable=0},
                new Department {DepartmentCode="JBPM",DepartmentDesc="Jabatan Bomba & Penyelamat Malaysia",Internal=2, Routeable=1},
                new Department {DepartmentCode="PDRM",DepartmentDesc="Polis Diraja Malaysia",Internal=2, Routeable=1},
                new Department {DepartmentCode="JKDM",DepartmentDesc="Jabatan Kastam Diraja Malaysia",Internal=2, Routeable=1},
            };
            departments.ForEach(s => context.Departments.Add(s));
            context.SaveChanges();

            var premisetypes = new List<PremiseType>
            {
                new PremiseType {PremiseDesc="Hotel, Kompleks Perniagaan"},
                new PremiseType {PremiseDesc="Kompleks Pejabat"},
                new PremiseType {PremiseDesc="Rumah Kedai"},
                new PremiseType {PremiseDesc="Kedai Pejabat"},
                new PremiseType {PremiseDesc="Bangunan Kerajaan"},
            };
            premisetypes.ForEach(s => context.PremiseTypes.Add(s));
            context.SaveChanges();

            var individuals = new List<Individual>
            {
                new Individual{FullName="Ali Bin Abu",MykadNo="710213-12-4820",NationalityID=1,PhoneNo="0108103140",AddressIC="No.3, Kg. Tg. Aru, Jalan Tg. Aru, 87000 W.P.Labuan",IndividualEmail="aliabu@yahoo.com",Gender=1},
                new Individual{FullName="Siti Aminah",MykadNo="610122-12-4933",NationalityID=1,PhoneNo="0112546778",AddressIC="Lot 20, Blok F, Taman Mutiara, 87000 W.P.Labuan",IndividualEmail="sitiaminah@gmail.com",Gender=2},
                new Individual{FullName="Chin Chee Kiong",MykadNo="500101-12-5129",NationalityID=1,PhoneNo="0148552370",AddressIC="Lot 13, Blok D, Jalan Merdeka, Pusat Bandar, 87000 W.P.Labuan",IndividualEmail="chinchee70@gmail.com",Gender=1},
                new Individual{FullName="Abdul Azis Hj Ibrahim",MykadNo="600501125629",NationalityID=1,Gender=1},
                new Individual{FullName="Arif Koh",MykadNo="H0392480",NationalityID=1,Gender=1},
                new Individual{FullName="Chan Chew Houi",MykadNo="790402086273",NationalityID=1,Gender=1},
                new Individual{FullName="Chua Kai Wen",MykadNo="760814125411",NationalityID=1,Gender=1},
                new Individual{FullName="Harilal Vasudevan",MykadNo="660823125343",NationalityID=1,Gender=1},
                new Individual{FullName="Hilary Koh Chin Kian @ Koh Chean Kan",MykadNo="551109125597",NationalityID=1,Gender=1},
                new Individual{FullName="Hj Mohd Ismail Bin Abdul Rahman",MykadNo="540521125093",NationalityID=1,Gender=1},
                new Individual{FullName="Imelda Binti Michael",MykadNo="840110125552",NationalityID=1,Gender=2},
            };
            individuals.ForEach(s => context.Individuals.Add(s));
            context.SaveChanges();


            var roletemplates = new List<ROLE>
            {
                new ROLE {ROLE_DESC="Public",DURATION=60 },
                new ROLE {ROLE_DESC="Desk Officer",DURATION=3 },
                new ROLE {ROLE_DESC="Clerk",DURATION=3 },
                new ROLE {ROLE_DESC="Supervisor",DURATION=3 },
                new ROLE {ROLE_DESC="Route Unit",DURATION=3 },
                new ROLE {ROLE_DESC="Director",DURATION=3 },
                new ROLE {ROLE_DESC="CEO",DURATION=3 },
                new ROLE {ROLE_DESC="Administrator",DURATION=3 },
            };
            roletemplates.ForEach(s => context.ROLEs.Add(s));
            context.SaveChanges();

            var AppStatus = new List<AppStatus>
            {
                new AppStatus {StatusDesc="Draft created" ,PercentProgress =1},
                new AppStatus {StatusDesc="Document Incomplete" ,PercentProgress =5},
                new AppStatus {StatusDesc="Submitted to clerk" ,PercentProgress =10},
                new AppStatus {StatusDesc="Processing by Clerk" ,PercentProgress =20},
                new AppStatus {StatusDesc="Route Unit" ,PercentProgress =30},
                new AppStatus {StatusDesc="Awaiting Director Response" ,PercentProgress =40},
                new AppStatus {StatusDesc="Meeting" ,PercentProgress =50},
                new AppStatus {StatusDesc="Awaiting CEO Approval" ,PercentProgress =60},
                new AppStatus {StatusDesc="Letter of Notification (Approved)" ,PercentProgress =70},
                new AppStatus {StatusDesc="Letter of Notification (Rejected)" ,PercentProgress =100},
                new AppStatus {StatusDesc="Letter of Notification (Approved with Terms & Conditions)" ,PercentProgress =70},
                new AppStatus {StatusDesc="Pending payment" ,PercentProgress =80},
                new AppStatus {StatusDesc="Paid" ,PercentProgress =90},
                new AppStatus {StatusDesc="Print License" ,PercentProgress =95},
                new AppStatus {StatusDesc="Complete" ,PercentProgress =100},
            };
            AppStatus.ForEach(s => context.AppStatus.Add(s));
            context.SaveChanges();

            var accesspages = new List<AccessPage>
            {
                new AccessPage {PageDesc="AccessPages",CrudLevel=0,ROLEID=1,ScreenId=1},
                new AccessPage {PageDesc="AccessPages",CrudLevel=0,ROLEID=2,ScreenId=1},
                new AccessPage {PageDesc="AccessPages",CrudLevel=2,ROLEID=3,ScreenId=1},
                new AccessPage {PageDesc="AccessPages",CrudLevel=3,ROLEID=4,ScreenId=1},
                new AccessPage {PageDesc="AccessPages",CrudLevel=3,ROLEID=5,ScreenId=1},
                new AccessPage {PageDesc="AccessPages",CrudLevel=4,ROLEID=6,ScreenId=1},
                new AccessPage {PageDesc="AccessPages",CrudLevel=4,ROLEID=7,ScreenId=1},
                new AccessPage {PageDesc="AccessPages",CrudLevel=4,ROLEID=8,ScreenId=1},

                new AccessPage {PageDesc="AdditionalInfos",CrudLevel=0,ROLEID=1,ScreenId=2},
                new AccessPage {PageDesc="AdditionalInfos",CrudLevel=0,ROLEID=2,ScreenId=2},
                new AccessPage {PageDesc="AdditionalInfos",CrudLevel=2,ROLEID=3,ScreenId=2},
                new AccessPage {PageDesc="AdditionalInfos",CrudLevel=3,ROLEID=4,ScreenId=2},
                new AccessPage {PageDesc="AdditionalInfos",CrudLevel=3,ROLEID=5,ScreenId=2},
                new AccessPage {PageDesc="AdditionalInfos",CrudLevel=4,ROLEID=6,ScreenId=2},
                new AccessPage {PageDesc="AdditionalInfos",CrudLevel=4,ROLEID=7,ScreenId=2},
                new AccessPage {PageDesc="AdditionalInfos",CrudLevel=4,ROLEID=8,ScreenId=2},

                new AccessPage {PageDesc="Attachment",CrudLevel=0,ROLEID=1,ScreenId=3},
                new AccessPage {PageDesc="Attachment",CrudLevel=0,ROLEID=2,ScreenId=3},
                new AccessPage {PageDesc="Attachment",CrudLevel=2,ROLEID=3,ScreenId=3},
                new AccessPage {PageDesc="Attachment",CrudLevel=3,ROLEID=4,ScreenId=3},
                new AccessPage {PageDesc="Attachment",CrudLevel=3,ROLEID=5,ScreenId=3},
                new AccessPage {PageDesc="Attachment",CrudLevel=4,ROLEID=6,ScreenId=3},
                new AccessPage {PageDesc="Attachment",CrudLevel=4,ROLEID=7,ScreenId=3},
                new AccessPage {PageDesc="Attachment",CrudLevel=4,ROLEID=8,ScreenId=3},

                new AccessPage {PageDesc="Administrator",CrudLevel=0,ROLEID=1,ScreenId=4},
                new AccessPage {PageDesc="Administrator",CrudLevel=0,ROLEID=2,ScreenId=4},
                new AccessPage {PageDesc="Administrator",CrudLevel=2,ROLEID=3,ScreenId=4},
                new AccessPage {PageDesc="Administrator",CrudLevel=3,ROLEID=4,ScreenId=4},
                new AccessPage {PageDesc="Administrator",CrudLevel=3,ROLEID=5,ScreenId=4},
                new AccessPage {PageDesc="Administrator",CrudLevel=4,ROLEID=6,ScreenId=4},
                new AccessPage {PageDesc="Administrator",CrudLevel=4,ROLEID=7,ScreenId=4},
                new AccessPage {PageDesc="Administrator",CrudLevel=4,ROLEID=8,ScreenId=4},

                new AccessPage {PageDesc="MasterSetup",CrudLevel=0,ROLEID=1,ScreenId=5},
                new AccessPage {PageDesc="MasterSetup",CrudLevel=0,ROLEID=2,ScreenId=5},
                new AccessPage {PageDesc="MasterSetup",CrudLevel=2,ROLEID=3,ScreenId=5},
                new AccessPage {PageDesc="MasterSetup",CrudLevel=3,ROLEID=4,ScreenId=5},
                new AccessPage {PageDesc="MasterSetup",CrudLevel=3,ROLEID=5,ScreenId=5},
                new AccessPage {PageDesc="MasterSetup",CrudLevel=4,ROLEID=6,ScreenId=5},
                new AccessPage {PageDesc="MasterSetup",CrudLevel=4,ROLEID=7,ScreenId=5},
                new AccessPage {PageDesc="MasterSetup",CrudLevel=4,ROLEID=8,ScreenId=5},

                new AccessPage {PageDesc="Inquiry",CrudLevel=0,ROLEID=1,ScreenId=6},
                new AccessPage {PageDesc="Inquiry",CrudLevel=0,ROLEID=2,ScreenId=6},
                new AccessPage {PageDesc="Inquiry",CrudLevel=2,ROLEID=3,ScreenId=6},
                new AccessPage {PageDesc="Inquiry",CrudLevel=3,ROLEID=4,ScreenId=6},
                new AccessPage {PageDesc="Inquiry",CrudLevel=3,ROLEID=5,ScreenId=6},
                new AccessPage {PageDesc="Inquiry",CrudLevel=4,ROLEID=6,ScreenId=6},
                new AccessPage {PageDesc="Inquiry",CrudLevel=4,ROLEID=7,ScreenId=6},
                new AccessPage {PageDesc="Inquiry",CrudLevel=4,ROLEID=8,ScreenId=6},

                new AccessPage {PageDesc="Reporting",CrudLevel=0,ROLEID=1,ScreenId=7},
                new AccessPage {PageDesc="Reporting",CrudLevel=1,ROLEID=2,ScreenId=7},
                new AccessPage {PageDesc="Reporting",CrudLevel=2,ROLEID=3,ScreenId=7},
                new AccessPage {PageDesc="Reporting",CrudLevel=3,ROLEID=4,ScreenId=7},
                new AccessPage {PageDesc="Reporting",CrudLevel=3,ROLEID=5,ScreenId=7},
                new AccessPage {PageDesc="Reporting",CrudLevel=4,ROLEID=6,ScreenId=7},
                new AccessPage {PageDesc="Reporting",CrudLevel=4,ROLEID=7,ScreenId=7},
                new AccessPage {PageDesc="Reporting",CrudLevel=4,ROLEID=8,ScreenId=7},

                new AccessPage {PageDesc="Individual",CrudLevel=0,ROLEID=1,ScreenId=8},
                new AccessPage {PageDesc="Individual",CrudLevel=3,ROLEID=2,ScreenId=8},
                new AccessPage {PageDesc="Individual",CrudLevel=3,ROLEID=3,ScreenId=8},
                new AccessPage {PageDesc="Individual",CrudLevel=3,ROLEID=4,ScreenId=8},
                new AccessPage {PageDesc="Individual",CrudLevel=3,ROLEID=5,ScreenId=8},
                new AccessPage {PageDesc="Individual",CrudLevel=4,ROLEID=6,ScreenId=8},
                new AccessPage {PageDesc="Individual",CrudLevel=4,ROLEID=7,ScreenId=8},
                new AccessPage {PageDesc="Individual",CrudLevel=4,ROLEID=8,ScreenId=8},

                new AccessPage {PageDesc="DeskOfficer",CrudLevel=0,ROLEID=1,ScreenId=9},
                new AccessPage {PageDesc="DeskOfficer",CrudLevel=1,ROLEID=2,ScreenId=9},
                new AccessPage {PageDesc="DeskOfficer",CrudLevel=0,ROLEID=3,ScreenId=9},
                new AccessPage {PageDesc="DeskOfficer",CrudLevel=0,ROLEID=4,ScreenId=9},
                new AccessPage {PageDesc="DeskOfficer",CrudLevel=0,ROLEID=5,ScreenId=9},
                new AccessPage {PageDesc="DeskOfficer",CrudLevel=0,ROLEID=6,ScreenId=9},
                new AccessPage {PageDesc="DeskOfficer",CrudLevel=0,ROLEID=7,ScreenId=9},
                new AccessPage {PageDesc="DeskOfficer",CrudLevel=0,ROLEID=8,ScreenId=9},

                new AccessPage {PageDesc="Profile",CrudLevel=0,ROLEID=1,ScreenId=10},
                new AccessPage {PageDesc="Profile",CrudLevel=2,ROLEID=2,ScreenId=10},
                new AccessPage {PageDesc="Profile",CrudLevel=2,ROLEID=3,ScreenId=10},
                new AccessPage {PageDesc="Profile",CrudLevel=3,ROLEID=4,ScreenId=10},
                new AccessPage {PageDesc="Profile",CrudLevel=3,ROLEID=5,ScreenId=10},
                new AccessPage {PageDesc="Profile",CrudLevel=4,ROLEID=6,ScreenId=10},
                new AccessPage {PageDesc="Profile",CrudLevel=4,ROLEID=7,ScreenId=10},
                new AccessPage {PageDesc="Profile",CrudLevel=4,ROLEID=8,ScreenId=10},

                new AccessPage {PageDesc="Process",CrudLevel=0,ROLEID=1,ScreenId=11},
                new AccessPage {PageDesc="Process",CrudLevel=1,ROLEID=2,ScreenId=11},
                new AccessPage {PageDesc="Process",CrudLevel=2,ROLEID=3,ScreenId=11},
                new AccessPage {PageDesc="Process",CrudLevel=3,ROLEID=4,ScreenId=11},
                new AccessPage {PageDesc="Process",CrudLevel=3,ROLEID=5,ScreenId=11},
                new AccessPage {PageDesc="Process",CrudLevel=4,ROLEID=6,ScreenId=11},
                new AccessPage {PageDesc="Process",CrudLevel=4,ROLEID=7,ScreenId=11},
                new AccessPage {PageDesc="Process",CrudLevel=4,ROLEID=8,ScreenId=11},

                new AccessPage {PageDesc="PremiseApplication",CrudLevel=1,ROLEID=1,ScreenId=13},
                new AccessPage {PageDesc="PremiseApplication",CrudLevel=4,ROLEID=2,ScreenId=13},
                new AccessPage {PageDesc="PremiseApplication",CrudLevel=4,ROLEID=3,ScreenId=13},
                new AccessPage {PageDesc="PremiseApplication",CrudLevel=4,ROLEID=4,ScreenId=13},
                new AccessPage {PageDesc="PremiseApplication",CrudLevel=1,ROLEID=5,ScreenId=13},
                new AccessPage {PageDesc="PremiseApplication",CrudLevel=4,ROLEID=6,ScreenId=13},
                new AccessPage {PageDesc="PremiseApplication",CrudLevel=4,ROLEID=7,ScreenId=13},
                new AccessPage {PageDesc="PremiseApplication",CrudLevel=4,ROLEID=8,ScreenId=13},

                new AccessPage {PageDesc="Application",CrudLevel=1,ROLEID=1,ScreenId=14},
                new AccessPage {PageDesc="Application",CrudLevel=4,ROLEID=2,ScreenId=14},
                new AccessPage {PageDesc="Application",CrudLevel=4,ROLEID=3,ScreenId=14},
                new AccessPage {PageDesc="Application",CrudLevel=4,ROLEID=4,ScreenId=14},
                new AccessPage {PageDesc="Application",CrudLevel=1,ROLEID=5,ScreenId=14},
                new AccessPage {PageDesc="Application",CrudLevel=4,ROLEID=6,ScreenId=14},
                new AccessPage {PageDesc="Application",CrudLevel=4,ROLEID=7,ScreenId=14},
                new AccessPage {PageDesc="Application",CrudLevel=4,ROLEID=8,ScreenId=14},
            };
            accesspages.ForEach(s => context.AccessPages.Add(s));
            context.SaveChanges();

            var users = new List<Users>
            {
                new Users {FullName="Abd Aziz Bin Hamzah",Username="aziz",Password="rGWQ/rZGq74=",Email="aziz.h@pl.gov.my", RoleTemplateID=6,DepartmentID=1},
                new Users {FullName="Soffiyan Bin Hadis",Username="soffiyan",Password="rGWQ/rZGq74=",Email="soffiyan.hadis@pl.gov.my", RoleTemplateID=4,DepartmentID=1},
                new Users {FullName="Hjh. Simai Binti Md Jamil",Username="simai",Password="rGWQ/rZGq74=",Email="simai@pl.gov.my", RoleTemplateID=3,DepartmentID=1},
                new Users {FullName="Suwardi Binti Muali",Username="suwardi",Password="rGWQ/rZGq74=",Email="suwardi.muali.pl@1govuc.gov.my", RoleTemplateID=2,DepartmentID=1},
                new Users {FullName="Adey Suhaimi Bin Suhaili",Username="adey",Password="rGWQ/rZGq74=",Email="adey.suhaimi.pl@1govuc.gov.my", RoleTemplateID=3,DepartmentID=1},
                new Users {FullName="Azean Irdawati Binti Wahid",Username="azean",Password="rGWQ/rZGq74=",Email="azean.wahid.pl@1govuc.gov.my", RoleTemplateID=3,DepartmentID=1},
                new Users {FullName="Kazlina Binti Kassim",Username="kazlina",Password="rGWQ/rZGq74=",Email="kazlina@yahoo.com", RoleTemplateID=3,DepartmentID=1},
                new Users {FullName="Johaniza Binti Jonait",Username="johaniza",Password="rGWQ/rZGq74=",Email="johaniza@yahoo.com", RoleTemplateID=3,DepartmentID=1},
                new Users {FullName="Rafidah Binti Mohd Isa",Username="rafidah",Password="rGWQ/rZGq74=",Email="rafidah@yahoo.com", RoleTemplateID=2,DepartmentID=1},
                new Users {FullName="Ahmad Jais Bin Halon",Username="ahmadjais",Password="rGWQ/rZGq74=",Email="ahmad.jais@yahoo.com", RoleTemplateID=2,DepartmentID=1},
                new Users {FullName="YBHG. Datuk Azhar Bin Ahmad",Username="kpe",Password="rGWQ/rZGq74=",Email="azharahmad@pl.gov.my", RoleTemplateID=7,DepartmentID=1},
                new Users {FullName="Mazalan Bin Hassin",Username="mazalan",Password="rGWQ/rZGq74=",Email="mazalan.hassin@pl.gov.my", RoleTemplateID=8,DepartmentID=1},
                new Users {FullName="R. Norasliana Binti Ramlee",Username="norasliana",Password="rGWQ/rZGq74=",Email="ana.ramli@pl.gov.my", RoleTemplateID=8,DepartmentID=1},
                new Users {FullName="Jabatan Bomba & Penyelamat Malaysia",Username="bomba",Password="rGWQ/rZGq74=",Email="jbpm_labuan.bomba@1govuc.gov.my", RoleTemplateID=5,DepartmentID=1},
                new Users {FullName="Wakil Bahagian Perancangan",Username="bpp",Password="rGWQ/rZGq74=",Email="bpp@pl.gov.my", RoleTemplateID=5,DepartmentID=3},
                new Users {FullName="Wakil Unit Kesihatan",Username="uks",Password="rGWQ/rZGq74=",Email="uks@pl.gov.my", RoleTemplateID=5,DepartmentID=5},
                new Users {FullName="Wakil Jabatan Penilaian",Username="jppph",Password="rGWQ/rZGq74=",Email="jppph@pl.gov.my", RoleTemplateID=5,DepartmentID=4},
                new Users {FullName="Ronny Jimmy",Username="ronny",Password="rGWQ/rZGq74=",Email="ronnyrtg@yahoo.com", RoleTemplateID=8,DepartmentID=1},
            };
            users.ForEach(s => context.Users.Add(s));
            context.SaveChanges();

            var requireddocs = new List<RequiredDoc>
            {
                new RequiredDoc {RequiredDocDesc="Borang Komposit bagi Permohonan Lesen Premis Perniagaan dan Iklan"},
                new RequiredDoc {RequiredDocDesc="Satu (1) Salinan Kad Pengenalan ATAU Pasport (depan dan belakang)"},
                new RequiredDoc {RequiredDocDesc="*Salinan Perakuan Pemerbadanan Syarikat/Perakuan Pendaftaran Syarikat/Perakuan Pendaftaran Perniagaan/Perakuan Pendaftaran Perkongsian Liabiliti Terhad (Borang 9, Borang 24 dan Borang 49) ATAU Sijil Pendaftaran Pertubuhan/Persatuan/Kelab/Badan Profesional"},
                new RequiredDoc {RequiredDocDesc="Lakaran Pelan Lokasi Perniagaan & 1 Gambar Premis (1 keping Pandangan hadapan/dalam premis)"},
                new RequiredDoc {RequiredDocDesc="Salinan Sijil Kelayakan Menduduki Bangunan (CF) ATAU Sijil Pematuhan (CCC/CFO) (Untuk bangunan baru siap/jika berkaitan)"},
                new RequiredDoc {RequiredDocDesc="Salinan hak milik/Perjanjian sewaan/Surat Kebenaran Pemilik/Geran Tanah@TOL (*mana yang berkaitan)"},
                new RequiredDoc {RequiredDocDesc="Salinan cukai taksiran terkini/Slip Bayaran Sewa Premis Majlis"},
                new RequiredDoc {RequiredDocDesc="Pengesahan Ejaan Penggunaan Bahasa pada Visual Iklan Premis oleh Dewan Bahasa dan Pustaka"},
                new RequiredDoc {RequiredDocDesc="Kelulusan Ubahsuai Bangunan/Permit Bangunan Sementara (jika berkaitan)"},
                new RequiredDoc {RequiredDocDesc="Surat Sokongan Bomba (jika berkaitan)"},
                new RequiredDoc {RequiredDocDesc="Surat Wakil & IC wakil pemohon lesen (jika berkaitan)"},
                new RequiredDoc {RequiredDocDesc="Tambahan dokumen dan pematuhan syarat & peraturan Pihak Berkuasa Melesen mengikut Aktiviti Perniagaan (Lampiran I & II) (jika berkaitan) "},
                new RequiredDoc {RequiredDocDesc="Borang D (Akaun Pendaftaran Perniagaan) mengikut Akta Pendaftaran Perniagaan 1956"},
                new RequiredDoc {RequiredDocDesc="Surat persetujuan/pengesahan daripada pemunya tanah atau bangunan"},
                new RequiredDoc {RequiredDocDesc="Sampul surat berukuran 9 x 4 inci dan Setem 30sen"},
                new RequiredDoc {RequiredDocDesc="Salinan lesen membuka tempat hiburan/lesen hiburan yang dahulu"},
                new RequiredDoc {RequiredDocDesc="Memorandum dan Artikel Persatuan dan Borang 49 (penyata yang memberikan butir-butir dalam Daftar Pengarah, Pengurus, dan Perubahan Butiran) mengikut Akta Syarikat 1965"},
                new RequiredDoc {RequiredDocDesc="Borang A (Pendaftaran Perniagaan) atau Borang B (Pendaftaran Penukaran Perniagaan)"},
                new RequiredDoc {RequiredDocDesc="Salinan yang diperakui Borang J (borang cukai pendapatan) bagi tempoh dua tahun yang lalu dan suatu salinan yang diperakui penyata akaun bank"},
                new RequiredDoc {RequiredDocDesc="Suatu salinan akuan berkanun seperti dalam Jadual C"},
                new RequiredDoc {RequiredDocDesc="Suatu salinan yang diperakui surat daripada pegawai polis seperti yang dikehendaki di bawah subperaturan 4(2)"},
                new RequiredDoc {RequiredDocDesc="Suatu salinan yang diperakui akaun beraudit atau akaun pengurusan yang terkini"},
                new RequiredDoc {RequiredDocDesc="Suatu salinan yang diperakui butir-butir dalam Borang 13 (jika ada), 24, 49 dan borang keuntungan tahunan terkini di bawah Akta Syarikat 1965 (bagi syarikat sahaja)"},

            };
            requireddocs.ForEach(s => context.RequiredDocs.Add(s));
            context.SaveChanges();

            var rds = new List<RD>
            {
                new RD {RD_DESC="Borang Komposit bagi Permohonan Lesen Premis Perniagaan dan Iklan"},
                new RD {RD_DESC="Satu (1) Salinan Kad Pengenalan ATAU Pasport (depan dan belakang)"},
                new RD {RD_DESC="*Salinan Perakuan Pemerbadanan Syarikat/Perakuan Pendaftaran Syarikat/Perakuan Pendaftaran Perniagaan/Perakuan Pendaftaran Perkongsian Liabiliti Terhad (Borang 9, Borang 24 dan Borang 49) ATAU Sijil Pendaftaran Pertubuhan/Persatuan/Kelab/Badan Profesional"},
                new RD {RD_DESC="Lakaran Pelan Lokasi Perniagaan & 1 Gambar Premis (1 keping Pandangan hadapan/dalam premis)"},
                new RD {RD_DESC="Salinan Sijil Kelayakan Menduduki Bangunan (CF) ATAU Sijil Pematuhan (CCC/CFO) (Untuk bangunan baru siap/jika berkaitan)"},
                new RD {RD_DESC="Salinan hak milik/Perjanjian sewaan/Surat Kebenaran Pemilik/Geran Tanah@TOL (*mana yang berkaitan)"},
                new RD {RD_DESC="Salinan cukai taksiran terkini/Slip Bayaran Sewa Premis Majlis"},
                new RD {RD_DESC="Pengesahan Ejaan Penggunaan Bahasa pada Visual Iklan Premis oleh Dewan Bahasa dan Pustaka"},
                new RD {RD_DESC="Kelulusan Ubahsuai Bangunan/Permit Bangunan Sementara (jika berkaitan)"},
                new RD {RD_DESC="Surat Sokongan Bomba (jika berkaitan)"},
                new RD {RD_DESC="Surat Wakil & IC wakil pemohon lesen (jika berkaitan)"},
                new RD {RD_DESC="Tambahan dokumen dan pematuhan syarat & peraturan Pihak Berkuasa Melesen mengikut Aktiviti Perniagaan (Lampiran I & II) (jika berkaitan) "},
                new RD {RD_DESC="Borang D (Akaun Pendaftaran Perniagaan) mengikut Akta Pendaftaran Perniagaan 1956"},
                new RD {RD_DESC="Surat persetujuan/pengesahan daripada pemunya tanah atau bangunan"},
                new RD {RD_DESC="Sampul surat berukuran 9 x 4 inci dan Setem 30sen"},
                new RD {RD_DESC="Salinan lesen membuka tempat hiburan/lesen hiburan yang dahulu"},
                new RD {RD_DESC="Memorandum dan Artikel Persatuan dan Borang 49 (penyata yang memberikan butir-butir dalam Daftar Pengarah, Pengurus, dan Perubahan Butiran) mengikut Akta Syarikat 1965"},
                new RD {RD_DESC="Borang A (Pendaftaran Perniagaan) atau Borang B (Pendaftaran Penukaran Perniagaan)"},
                new RD {RD_DESC="Salinan yang diperakui Borang J (borang cukai pendapatan) bagi tempoh dua tahun yang lalu dan suatu salinan yang diperakui penyata akaun bank"},
                new RD {RD_DESC="Suatu salinan akuan berkanun seperti dalam Jadual C"},
                new RD {RD_DESC="Suatu salinan yang diperakui surat daripada pegawai polis seperti yang dikehendaki di bawah subperaturan 4(2)"},
                new RD {RD_DESC="Suatu salinan yang diperakui akaun beraudit atau akaun pengurusan yang terkini"},
                new RD {RD_DESC="Suatu salinan yang diperakui butir-butir dalam Borang 13 (jika ada), 24, 49 dan borang keuntungan tahunan terkini di bawah Akta Syarikat 1965 (bagi syarikat sahaja)"},

            };
            rds.ForEach(s => context.RD.Add(s));
            context.SaveChanges();


            var additionaldocs = new List<AdditionalDoc>
            {
                new AdditionalDoc { DocDesc = "Salinan Sijil Suntikan TY2 (Pengendalian Makanan)(jika ada)" },
                new AdditionalDoc { DocDesc = "Salinan Sijil Kursus Pengendalian Makanan" },
                new AdditionalDoc { DocDesc = "Sijil Pendaftaran Premis Makanan daripada KKM" },
                new AdditionalDoc { DocDesc = "Salinan Resit (bukti) pembelian alat penapis minyak" },
                new AdditionalDoc { DocDesc = "(Salun rambut, Gunting Rambut, SPA, Refleksologi dll) Pengesahan kesihatan bagi pengusaha dan pekerja daripada pengamal perubatan yang diiktiraf (jika ada)" },
                new AdditionalDoc { DocDesc = "Surat sokongan Bahagian Perkhidmatan Farmasi dan Jabatan Kesihatan Negeri" },
                new AdditionalDoc { DocDesc = "Salinan Sijil Pendaftaran daripada Jabatan Pelajaran Negeri/Kementerian Pendidikan bagi TADIKA/PUSAT TUISYEN/PUSAT PENGAJIAN" },
                new AdditionalDoc { DocDesc = "Salinan Perakuan Pendaftaran sementara daripada Jabatan Kebajikan Masyarakat (JKM) bagi TASKA/PUSAT JAGAAN" },
                new AdditionalDoc { DocDesc = "Salinan Sijil Suntikan TY2 (Pengendalian Makanan) bagi premis yang menyediakan makanan (jika berkaitan)" },
                new AdditionalDoc { DocDesc = "Perakuan Mesin Jentera daripada Jabatan Keselamatan & Kesihatan Pekerjaan (JKKP) (jika berkaitan)" },
                new AdditionalDoc { DocDesc = "Kelulusan daripada Jabatan Alam Sekitar (bagi aktiviti yang menghasilkan sisa buangan yang membahayakan persekitaran)" },
                new AdditionalDoc { DocDesc = "Kelulusan daripada Jabatan Bomba dan Penyelamat Malaysia" },
                new AdditionalDoc { DocDesc = "Tambahan dokumen sokongan mengikut aktiviti perkilangan seperti lampiran II (jika berkaitan)" },
                new AdditionalDoc { DocDesc = "Salinan visual ilustrasi iklan yang diluluskan oleh DBP dan mengikut spesifikasi yang ditetapkan PBT (memastikan penggunaan Bahasa Kebangsaan lebih utama & lebih besar dari bahasa lain di Malaysia/penggunaan Bahasa asing tidak dibenarkan" },
                new AdditionalDoc { DocDesc = "Gambar lokasi@kedudukan tempat pemasangan iklan premis" },
                new AdditionalDoc { DocDesc = "Salinan Kad Pengenalan" },
                new AdditionalDoc { DocDesc = "Gambar berukuran pasport (2 keping)" },
                new AdditionalDoc { DocDesc = "Salinan Pendaftaran Perniagaan (ROC/ROB/SSM)" },
                new AdditionalDoc { DocDesc = "Gambar dan pelan lokasi tapak menjaja (penjaja statik)" },
                new AdditionalDoc { DocDesc = "Salinan Geran Pendaftaran Kenderaan/Kelulusan JPJ/Surat kebenaran pemilik kenderaan jika bukan kenderaan milik sendiri/lesen memandu (yang melibatkan kenderaan sahaja)" },
                new AdditionalDoc { DocDesc = "Surat Tawaran/Pengesahan Penganjur Pihak Berkuasa Tempatan *jika berkenaan" },
                new AdditionalDoc { DocDesc = "Perakuan Pendaftaran daripada Jabatan Pertanian" },
                new AdditionalDoc { DocDesc = "Lesen runcit barangan terkawal daripada KPDNKK" },
                new AdditionalDoc { DocDesc = "Lesen Mengilang bagi Barang Kawalan Berjadual Di bawah Akta Kawalan Bekalan 1961 (CSA)" },
                new AdditionalDoc { DocDesc = "Lesen Membuat/Membaiki/Menjual Alat-alat Timbang & Sukat Di bawah Akta Timbang & Sukat 1972" },
                new AdditionalDoc { DocDesc = "Kelulusan untuk Mengilang Papan Suis" },
                new AdditionalDoc { DocDesc = "Mengilang Peralatan Gas" },
                new AdditionalDoc { DocDesc = "Lesen Biobahan Api" },
                new AdditionalDoc { DocDesc = "Lesen Membina Kilang" },
                new AdditionalDoc { DocDesc = "Lesen Pekebun Kecil" },
                new AdditionalDoc { DocDesc = "Lesen Estet" },
                new AdditionalDoc { DocDesc = "Lesen Mengawet Tembakau" },
                new AdditionalDoc { DocDesc = "Lesen Mengilang Tembakau atau Keluaran Tembakau" },
                new AdditionalDoc { DocDesc = "Lesen Pembuatan Produk Getah" },
                new AdditionalDoc { DocDesc = "Sijil Pengiktirafan Pendaftaran Bengkel Kejuruteraan (Bina Badan/Pengubahsuaian Teknikal Kenderaan Perdagangan)" },
                new AdditionalDoc { DocDesc = "Kelulusan Pendaftaran Kilang Kenderaan Perdagangan Bina Semula (Rebuilt)" },
                new AdditionalDoc { DocDesc = "Pendaftaran Bengkel Kejuruteraan (pemasangan dan pengubahsuaian teknikal bagi CNG/NGV)" },
                new AdditionalDoc { DocDesc = "Lesen bagi menduduki dan menggunakan Premis yang ditetapkan (Minyak Kelapa Sawit Mentah" },
                new AdditionalDoc { DocDesc = "Lesen bagi menduduki dan menggunakan Premis yang ditetapkan (Getah Asli Mentah)" },
                new AdditionalDoc { DocDesc = "Lesen Industri berasaskan kayu (Kilang Papan, Plywood, Moulding, Perabot dll)" },
                new AdditionalDoc { DocDesc = "Lesen Pengilang Keluaran Berdaftar (Farmaseutikal)" },
                new AdditionalDoc { DocDesc = "Skim Pensijilan Good Manufacturing Practice (GMP), HACCP" },
                new AdditionalDoc { DocDesc = "Lesen Kilang Padi Komersial" },
                new AdditionalDoc { DocDesc = "Sijil Pendaftaran Pembuat Makanan Haiwan atau Bahan Tambahan Makanan Haiwan" },
                new AdditionalDoc { DocDesc = "Pendaftaran Kilang Nanas dan Kilang Kecil" },
                new AdditionalDoc { DocDesc = "Permit mengimport/membeli/memakai/memiliki/mengilang bagi membuat dan menjual baju kalis peluru" },
                new AdditionalDoc { DocDesc = "Lesen Senjata Api" },
                new AdditionalDoc { DocDesc = "Lesen Cakera Optik di bawah Akta Cakera Optik 2000" },
                new AdditionalDoc { DocDesc = "kelulusan Jawatankuasa Perdagangan Pengedaran (bagi pemilik bukan warganegara sahaja)" },
                new AdditionalDoc { DocDesc = "Lesen Pedagang Koko" },
                new AdditionalDoc { DocDesc = "Lesen Peniaga/Taxidermi" },
                new AdditionalDoc { DocDesc = "Lesen Runcit Beras" },
                new AdditionalDoc { DocDesc = "Lesen Akta Jualan Langsung" },
                new AdditionalDoc { DocDesc = "Permohonan membawa masuk dan penjualan 'pepper spray/security spray'" },
                new AdditionalDoc { DocDesc = "Lesen Magazin Bahan Letupan" },
                new AdditionalDoc { DocDesc = "Lesen daripada FINAS" },
                new AdditionalDoc { DocDesc = "Perakuan Pendaftaran Farmasi" },
                new AdditionalDoc { DocDesc = "Perakuan Kelayakan Optometri" },
                new AdditionalDoc { DocDesc = "Permit berniaga Peralatan dan Pakaian Seragam Polis" },
                new AdditionalDoc { DocDesc = "Lesen Perniagaan Pengendalian Pelancongan dan Perniagaan Agensi Pengembaraan" },
                new AdditionalDoc { DocDesc = "Pendaftaran Syarikat Perunding dan Jurutera Profesional" },
                new AdditionalDoc { DocDesc = "Lesen Pemaju Perumahan Permit Iklan & Jualan Baru Pemaju" },
                new AdditionalDoc { DocDesc = "Lesen Agensi Pekerjaan Swasta" },
                new AdditionalDoc { DocDesc = "Lesen Agensi Persendirian" },
                new AdditionalDoc { DocDesc = "Pendaftaran Syarikat Amalan Perunding Kejuruteraan" },
                new AdditionalDoc { DocDesc = "Sijil Perakuan Pendaftaran Kontraktor" },
                new AdditionalDoc { DocDesc = "Kelulusan Jabatan Pendidikan Negeri" },
                new AdditionalDoc { DocDesc = "Pendaftaran IPTS" },
                new AdditionalDoc { DocDesc = "Permit Sekolah/Institut Memandu" },
                new AdditionalDoc { DocDesc = "Perakuan Kelulusan Sekolah Latihan Penerbangan" },
                new AdditionalDoc { DocDesc = "Lesen Institut Latihan Pelancongan" },
                new AdditionalDoc { DocDesc = "Pengiktirafan Sekolah Latihan Pengendali Makanan (SLPM)" },
                new AdditionalDoc { DocDesc = "Kelulusan Jabatan Pendidikan Negeri" },
                new AdditionalDoc { DocDesc = "Perakuan Pendaftaran Taman Asuhan Kanak-kanak" },
                new AdditionalDoc { DocDesc = "Perakuan Pendaftaran Pusat Jagaan" },
                new AdditionalDoc { DocDesc = "Pendaftaran Hotel" },
                new AdditionalDoc { DocDesc = "Pendaftaran Premis Penginapan Pelancong" },
                new AdditionalDoc { DocDesc = "Perakuan Pendaftaran untuk Menubuhkan/Menyenggarakan/Mengendalikan/Menyediakan Klinik Perubatan/Pergigian Swasta" },
                new AdditionalDoc { DocDesc = "Perakuan Kelulusan untuk Menubuhkan/Menyenggarakan Kemudahan/Perkhidmatan Jagaan Kesihatan Swasta" },
                new AdditionalDoc { DocDesc = "Lesen untuk Mengendalikan/Menyediakan Kemudahan/Perkhidmatan Jagaan Kesihatan Swasta" },
                new AdditionalDoc { DocDesc = "Pendaftaran Penuh Optometris" },
                new AdditionalDoc { DocDesc = "Sijil Amanah Tahunan" },
                new AdditionalDoc { DocDesc = "Sijil Pendaftaran Pertubuhan Perbadanan" },
                new AdditionalDoc { DocDesc = "Perakuan Kelayakan Optometri" },
                new AdditionalDoc { DocDesc = "Perakuan Pendaftaran Pengamal Pergigian" },
                new AdditionalDoc { DocDesc = "Kebenaran Di Bawah Petroleum Development Act 1974 (PDA 1,2,3 dan 4)" },
                new AdditionalDoc { DocDesc = "Lesen Menjual Barang-barang Lusuh" },
                new AdditionalDoc { DocDesc = "Lesen Menyimpan Hidupan Liar yang Dilindungi" },
                new AdditionalDoc { DocDesc = "Lesen Penggudangan" },
                new AdditionalDoc { DocDesc = "Lesen Penggurup Wang" },
                new AdditionalDoc { DocDesc = "Lesen Pembrokeran Wang" },
                new AdditionalDoc { DocDesc = "Lesen Perniagaan Perkhidmatan Wang" },
                new AdditionalDoc { DocDesc = "lesen Insurans" },
                new AdditionalDoc { DocDesc = "Lesen Mengimport Rokok dan Minuman Keras" },
                new AdditionalDoc { DocDesc = "Lesen Perfileman Malaysia" },
                new AdditionalDoc { DocDesc = "Lesen Borong Beras" },
                new AdditionalDoc { DocDesc = "Lesen Control Supply Act (CSA)" },
                new AdditionalDoc { DocDesc = "Lesen Pengisar" },
                new AdditionalDoc { DocDesc = "Lesen Tapak Semaian Getah (Nurseri)" },
                new AdditionalDoc { DocDesc = "Lesen Mesin Cetak" },
                new AdditionalDoc { DocDesc = "Lesen Pemberi Pinjam Wang" },
                new AdditionalDoc { DocDesc = "lesen Pemegang Pajak Gadai" },
                new AdditionalDoc { DocDesc = "Surat Kebenaran Agen Penghantaran" },
                new AdditionalDoc { DocDesc = "Lesen Perkhidmatan Kurier" },
                new AdditionalDoc { DocDesc = "Lesen Rumah Sembelih Swasta" },
                new AdditionalDoc { DocDesc = "Pendaftaran Syarikat Filem" },
                new AdditionalDoc { DocDesc = "Lesen Pengendali Lori" },
                new AdditionalDoc { DocDesc = "Lesen Pengendali Bas" },
                new AdditionalDoc { DocDesc = "Lesen Perusahaan atau Penyediaan Perkhidmatan Pengurusan Pembersihan Awam" },
                new AdditionalDoc { DocDesc = "Lesen Perusahaan atau Penyediaan Perkhidmatan Pemungutan bagi Sisa Pepejal Isi Rumah, Sisa Pepejal Awam, Sisa Pepejal Keinstitusian Awam dan Sisa Pepejal yang Serupa" },
                new AdditionalDoc { DocDesc = "Lesen Perkhidmatan Pengangkutan dengan Long Haulage" },
                new AdditionalDoc { DocDesc = "Lesen Judi" },
            };
			additionaldocs.ForEach(s => context.AdditionalDocs.Add(s));
            context.SaveChanges();

            var companies = new List<Company>
            {
                new Company {RegistrationNo="75278-T",CompanyName="Chin Recycle",CompanyPhone="087430010",SSMRegDate=DateTime.Parse("01-01-2018"),SSMExpDate=DateTime.Parse("31-12-2018")},
                new Company {RegistrationNo="801234-V",CompanyName="Kejora Bersatu Sdn Bhd",CompanyPhone="087450690",SSMRegDate=DateTime.Parse("01-06-2017"),SSMExpDate=DateTime.Parse("30-06-2018")},
                new Company {RegistrationNo="991345-V",CompanyName="Kentucky Fried Chicken (KFC)",CompanyPhone="087421090",SSMRegDate=DateTime.Parse("11-Aug-2018"),SSMExpDate=DateTime.Parse("12-Aug-2019")},
                new Company {RegistrationNo="129074-M",CompanyName="Marry Brown",CompanyPhone="087-411555",SSMRegDate=DateTime.Parse("13-Oct-2018"),SSMExpDate=DateTime.Parse("12-Oct-2019")},
                new Company {RegistrationNo="203976-T",CompanyName="Borneo Combat Gym",CompanyPhone="011-3516 1698",SSMRegDate=DateTime.Parse("01-Feb-2018"),SSMExpDate=DateTime.Parse("01-Feb-2019")},
                new Company {RegistrationNo="987264-H",CompanyName="Dorsett Grand Labuan",CompanyPhone="+608 7422 000",SSMRegDate=DateTime.Parse("01-Jan-2018"),SSMExpDate=DateTime.Parse("01-Jan-2019")},
                new Company {RegistrationNo="987264-H",CompanyName="Red Tomato Hotel",CompanyPhone="087-412 963",SSMRegDate=DateTime.Parse("01-Nov-2018"),SSMExpDate=DateTime.Parse("01-Nov-2019")},
                new Company {RegistrationNo="355817-T",CompanyName="Olympic Pool & Snooker",CompanyPhone="087-467522",SSMRegDate=DateTime.Parse("01-Mar-2018"),SSMExpDate=DateTime.Parse("01-Mar-2019")},
                new Company {RegistrationNo="188846-T",CompanyName="Kedai Gunting Rambut Wahab",SSMRegDate=DateTime.Parse("01-Feb-2018"),SSMExpDate=DateTime.Parse("01-Feb-2019")},
                new Company {RegistrationNo="203433-V",CompanyName="Klinik Suria (Labuan) Sdn. Bhd.",CompanyPhone="087-504 969",SSMRegDate=DateTime.Parse("01-Apr-2018"),SSMExpDate=DateTime.Parse("01-Apr-2019")},
                new Company {RegistrationNo="203433-V",CompanyName="Wong Brothers Workshop & Service Sdn. Bhd.",CompanyPhone="087-414 784",SSMRegDate=DateTime.Parse("21-Jan-2018"),SSMExpDate=DateTime.Parse("21-Jan-2019")},
                new Company {RegistrationNo="203433-V",CompanyName="Hobby Mix.",CompanyPhone="087-429 428",SSMRegDate=DateTime.Parse("31-Jan-2018"),SSMExpDate=DateTime.Parse("31-Jan-2019")},
                new Company {CompanyName="Thirumurugan Temple"},
                new Company {CompanyName="Jabatan Kerja Raya",CompanyPhone="087-414 040"},
                new Company {CompanyName="Sekolah Menengah Sains Labuan",CompanyPhone="(+60) 87 461525"},
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

            var zones = new List<Zone>
            {
                new Zone {ZoneCode="11",ZoneDesc="Bandar-Perdagangan"},
                new Zone {ZoneCode="12",ZoneDesc="Bandar-Perindustrian"},
                new Zone {ZoneCode="13",ZoneDesc="Bandar-Tanah Kosong"},
                new Zone {ZoneCode="14",ZoneDesc="Bandar-Perumahan dalam taman perumahan"},
                new Zone {ZoneCode="15",ZoneDesc="Bandar-Perumahan di luar taman perumahan"},
                new Zone {ZoneCode="1A",ZoneDesc="SMK-Perdagangan"},
                new Zone {ZoneCode="1B",ZoneDesc="SMK-Perindustrian"},
                new Zone {ZoneCode="1D",ZoneDesc="SMK-Tanah Kosong"},
                new Zone {ZoneCode="1E",ZoneDesc="SMK-Perumahan dalam taman perumahan"},
                new Zone {ZoneCode="1F",ZoneDesc="SMK-Perumahan di luar taman perumahan"},
            };
            zones.ForEach(s => context.Zones.Add(s));
            context.SaveChanges();

            var locations = new List<Location>
            {
                new Location {LocationCode="01",LocationDesc="Jalan Tun Mustapha"},
                new Location {LocationCode="02",LocationDesc="Jalan Dewan"},
                new Location {LocationCode="03",LocationDesc="Jalan Merdeka"},
                new Location {LocationCode="04",LocationDesc="Jalan Bahasa"},
                new Location {LocationCode="05",LocationDesc="Jalan Bunga Kenanga"},
                new Location {LocationCode="06",LocationDesc="Jalan Bunga Raya"},
                new Location {LocationCode="07",LocationDesc="Jalan Perpaduan"},
                new Location {LocationCode="08",LocationDesc="Jalan Bunga Tanjung"},
                new Location {LocationCode="09",LocationDesc="Jalan OKK Awang Besar"},
                new Location {LocationCode="10",LocationDesc="Jalan Muhibah"},
            };
            locations.ForEach(s => context.Locations.Add(s));
            context.SaveChanges();

            var roads = new List<Road>
            {
                new Road {RoadCode="001",RoadDesc="Jalan Tun Mustapha"},
                new Road {RoadCode="002",RoadDesc="Jalan Dewan"},
                new Road {RoadCode="003",RoadDesc="Jalan Merdeka"},
                new Road {RoadCode="004",RoadDesc="Jalan Bahasa"},
                new Road {RoadCode="005",RoadDesc="Jalan Bunga Kenanga"},
                new Road {RoadCode="006",RoadDesc="Jalan Bunga Raya"},
                new Road {RoadCode="007",RoadDesc="Jalan Perpaduan"},
                new Road {RoadCode="008",RoadDesc="Jalan Bunga Tanjung"},
                new Road {RoadCode="009",RoadDesc="Jalan OKK Awang Besar"},
                new Road {RoadCode="010",RoadDesc="Jalan Tanjung Pasir"},
            };
            roads.ForEach(s => context.Roads.Add(s));
            context.SaveChanges();


            var entmtCodes = new List<E_CODE>
            {
                new E_CODE {E_GROUPID=1,E_C_DESC="Tidak melebihi 200 kerusi",E_C_FEE=400.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=1,E_C_DESC="200 hingga 400 kerusi",E_C_FEE=800.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=1,E_C_DESC="400 hingga 600 kerusi",E_C_FEE=1000.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=1,E_C_DESC="600 hingga 800 kerusi",E_C_FEE=1400.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=1,E_C_DESC="800 hingga 1,000 kerusi",E_C_FEE=1600.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=1,E_C_DESC="1,000 hingga 1,200 kerusi",E_C_FEE=1800.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=1,E_C_DESC="Lebih 1,200 kerusi",E_C_FEE=2000.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=1,E_C_DESC="Lesen Sementara",E_C_FEE=10.0f,E_C_PERIOD=4,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=2,E_C_DESC="Tidak melebihi 200 kerusi",E_C_FEE=400.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=2,E_C_DESC="Melebihi 200 kerusi tetapi tidak melebihi 400 kerusi",E_C_FEE=900.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=2,E_C_DESC="Melebihi 400 kerusi tetapi tidak melebihi 600 kerusi",E_C_FEE=1100.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=2,E_C_DESC="Melebihi 600 kerusi tetapi tidak melebihi 800 kerusi",E_C_FEE=1550.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=2,E_C_DESC="Melebihi 800 kerusi tetapi tidak melebihi 1,000 kerusi",E_C_FEE=1800.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=2,E_C_DESC="Melebihi 1,000 kerusi tetapi tidak melebihi 1,200 kerusi",E_C_FEE=2000.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=2,E_C_DESC="Lebih 1,200 kerusi",E_C_FEE=2200.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=2,E_C_DESC="Lesen Sementara",E_C_FEE=20.0f,E_C_PERIOD=4,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=3,E_C_DESC="Luas lantai tidak melebihi 30 meter persegi",E_C_FEE=300.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=3,E_C_DESC="Luas lantai melebihi 30 meter persegi tetapi tidak melebihi 60 meter persegi",E_C_FEE=500.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=3,E_C_DESC="Luas lantai melebihi 60 meter persegi tetapi tidak melebihi 90 meter persegi",E_C_FEE=800.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=3,E_C_DESC="Lebih 90 meter persegi",E_C_FEE=1100.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=4,E_C_DESC="Pusat Hiburan/Taman Hiburan (Luar Bangunan)",E_C_FEE=10.0f,E_C_PERIOD=4,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=5,E_C_DESC="Lorong Boling",E_C_FEE=1100.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=6,E_C_DESC="Gelanggang Luncur",E_C_FEE=1000.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=7,E_C_DESC="Mesin Hiburan (Kiddy Rides/Juke Box) di tempat selain daripada Pusat Hiburan",E_C_O_FEE=20.0f,E_C_O_NAME="mesin",E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=8,E_C_DESC="Luas lantai tidak melebihi 30 meter persegi",E_C_FEE=400.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=8,E_C_DESC="30 hingga 60 meter persegi",E_C_FEE=600.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=8,E_C_DESC="60 hingga 90 meter persegi",E_C_FEE=900.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=8,E_C_DESC="Lebih 90 meter persegi",E_C_FEE=1200.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=8,E_C_DESC="Kabaret - Lebih 90 meter persegi",E_C_FEE=2200.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=9,E_C_DESC="Tidak melebihi 5 meja",E_C_FEE=500.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=9,E_C_DESC="Melebihi 5 meja tetapi tidak melebihi 10 meja",E_C_FEE=1000.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=9,E_C_DESC="Melebihi 10 meja tetapi tidak melebihi 20 meja",E_C_FEE=1500.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=9,E_C_DESC="Melebihi 20 meja tetapi tidak melebihi 30 meja",E_C_FEE=2000.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=9,E_C_DESC="Melebihi 30 meja tetapi tidak melebihi 40 meja",E_C_FEE=2500.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=9,E_C_DESC="Melebihi 40 meja tetapi tidak melebihi 50 meja",E_C_FEE=3000.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=9,E_C_DESC="Melebihi 50 meja",E_C_B_FEE=3000.0f,E_C_O_FEE=50.0f,E_C_O_NAME="meja",E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=10,E_C_DESC="Tidak melebihi 1000 kerusi",E_C_FEE=400.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=10,E_C_DESC="Melebihi 1000 kerusi",E_C_FEE=800.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=10,E_C_DESC="Lesen sementara",E_C_FEE=50.0f,E_C_PERIOD=4,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=11,E_C_DESC="Luas lantai tidak melebihi 100 meter persegi",E_C_FEE=100.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=11,E_C_DESC="Luas lantai melebihi 100 meter persegi tetapi tidak melebihi 150 meter persegi",E_C_FEE=200.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=11,E_C_DESC="Luas lantai melebihi 150 meter persegi tetapi tidak melebihi 200 meter persegi",E_C_FEE=300.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=11,E_C_DESC="Luas lantai melebihi 200 meter persegi",E_C_FEE=500.0f,E_C_PERIOD=1,E_C_PERIOD_Q=1},
                new E_CODE {E_GROUPID=11,E_C_DESC="Lesen sementara",E_C_FEE=10.0f,E_C_PERIOD=4,E_C_PERIOD_Q=1},
            };
            entmtCodes.ForEach(s => context.E_CODEs.Add(s));
            context.SaveChanges();

            var btlinkreqdocs = new List<BTLinkReqDoc>
            {
                new BTLinkReqDoc {BusinessTypeID=1,RequiredDocID=1},
                new BTLinkReqDoc {BusinessTypeID=1,RequiredDocID=2},
                new BTLinkReqDoc {BusinessTypeID=1,RequiredDocID=3},
                new BTLinkReqDoc {BusinessTypeID=2,RequiredDocID=1},
                new BTLinkReqDoc {BusinessTypeID=2,RequiredDocID=2},
                new BTLinkReqDoc {BusinessTypeID=2,RequiredDocID=3},
                new BTLinkReqDoc {BusinessTypeID=3,RequiredDocID=1},
                new BTLinkReqDoc {BusinessTypeID=3,RequiredDocID=2},
                new BTLinkReqDoc {BusinessTypeID=3,RequiredDocID=3},
                new BTLinkReqDoc {BusinessTypeID=4,RequiredDocID=1},
                new BTLinkReqDoc {BusinessTypeID=4,RequiredDocID=2},
                new BTLinkReqDoc {BusinessTypeID=4,RequiredDocID=3},
                new BTLinkReqDoc {BusinessTypeID=5,RequiredDocID=1},
                new BTLinkReqDoc {BusinessTypeID=5,RequiredDocID=2},
                new BTLinkReqDoc {BusinessTypeID=5,RequiredDocID=3},
                new BTLinkReqDoc {BusinessTypeID=6,RequiredDocID=1},
                new BTLinkReqDoc {BusinessTypeID=6,RequiredDocID=2},
                new BTLinkReqDoc {BusinessTypeID=6,RequiredDocID=3},
            };
            btlinkreqdocs.ForEach(s => context.PALinkReqDocs.Add(s));
            context.SaveChanges();

            var btls = new List<BT_L_RD>
            {
                new BT_L_RD {BT_ID=1,RD_ID=1},
                new BT_L_RD {BT_ID=1,RD_ID=2},
                new BT_L_RD {BT_ID=1,RD_ID=3},
                new BT_L_RD {BT_ID=2,RD_ID=1},
                new BT_L_RD {BT_ID=2,RD_ID=2},
                new BT_L_RD {BT_ID=2,RD_ID=3},
                new BT_L_RD {BT_ID=3,RD_ID=1},
                new BT_L_RD {BT_ID=3,RD_ID=2},
                new BT_L_RD {BT_ID=3,RD_ID=3},
                new BT_L_RD {BT_ID=4,RD_ID=1},
                new BT_L_RD {BT_ID=4,RD_ID=2},
                new BT_L_RD {BT_ID=4,RD_ID=3},
                new BT_L_RD {BT_ID=5,RD_ID=1},
                new BT_L_RD {BT_ID=5,RD_ID=2},
                new BT_L_RD {BT_ID=5,RD_ID=3},
                new BT_L_RD {BT_ID=6,RD_ID=1},
                new BT_L_RD {BT_ID=6,RD_ID=2},
                new BT_L_RD {BT_ID=6,RD_ID=3},
            };
            btls.ForEach(s => context.BT_L_RD.Add(s));
            context.SaveChanges();

            var bclinkads = new List<BCLinkAD>
            {
                new BCLinkAD {BusinessCodeID=1,AdditionalDocID=1},
                new BCLinkAD {BusinessCodeID=1,AdditionalDocID=2},
                new BCLinkAD {BusinessCodeID=2,AdditionalDocID=3},
                new BCLinkAD {BusinessCodeID=2,AdditionalDocID=4},
            };
            bclinkads.ForEach(s => context.BCLinkAD.Add(s));
            context.SaveChanges();

            

            
        }
    }
}
