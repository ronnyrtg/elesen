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

            var sector = new List<SECTOR>
            {
                new SECTOR {SECTORID=1,SECTORDESC="Tred"},
                new SECTOR {SECTORID=2,SECTORDESC="Stor"},
                new SECTOR {SECTORID=3,SECTORDESC="Perindustrian"},
                new SECTOR {SECTORID=4,SECTORDESC="Bengkel"},
                new SECTOR {SECTORID=5,SECTORDESC="Pertanian/Penternakan"},
                new SECTOR {SECTORID=6,SECTORDESC="Lain-lain"},
            };
            sector.ForEach(s => context.SECTORs.Add(s));
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
            bts.ForEach(s => context.BTs.Add(s));
            context.SaveChanges();

            var race = new List<RACE_M>
            {
                new RACE_M {RACE_DESC="Melayu"},
                new RACE_M {RACE_DESC="Cina"},
                new RACE_M {RACE_DESC="India"},
                new RACE_M {RACE_DESC="Bumiputra Sabah"},
                new RACE_M {RACE_DESC="Bumiputra Sarawak"},
                new RACE_M {RACE_DESC="Bangsa dari luar negara Malaysia"},
            };
            race.ForEach(s => context.RACEs.Add(s));
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
                new BC {LIC_TYPEID=10,C_R="P08",C_R_DESC="Hiburan di Pub/Coffee House/Lounge/Disko/Dewan Tarian - Muzik dan nyanyian sehingga 12 malam",O_FEE=4.0f,O_NAME="sehingga 12 malam",PERIOD=4,PERIOD_Q=1},
                new BC {LIC_TYPEID=10,C_R="P09",C_R_DESC="Hiburan di Pub/Coffee House/Lounge/Disko/Dewan Tarian - Muzik dan nyanyian selepas 12 malam",O_FEE=8.0f,O_NAME="selepas 12 malam",PERIOD=4,PERIOD_Q=1},
                new BC {LIC_TYPEID=10,C_R="P10",C_R_DESC="Hiburan di Pub/Coffee House/Lounge/Disko/Dewan Tarian - Tarian sehingga 12 malam",O_FEE=4.0f,O_NAME="sehingga 12 malam",PERIOD=4,PERIOD_Q=1},
                new BC {LIC_TYPEID=10,C_R="P11",C_R_DESC="Hiburan di Pub/Coffee House/Lounge/Disko/Dewan Tarian - Tarian selepas 12 malam",O_FEE=14.0f,O_NAME="selepas 12 malam",PERIOD=4,PERIOD_Q=1},
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

            var entmtCodes = new List<E_P_FEE>
            {
                new E_P_FEE {E_P_DESC="Oditorium/Dewan",E_S_DESC="Tidak melebihi 200 kerusi",E_S_FEE=400.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Oditorium/Dewan",E_S_DESC="200 hingga 400 kerusi",E_S_FEE=800.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Oditorium/Dewan",E_S_DESC="400 hingga 600 kerusi",E_S_FEE=1000.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Oditorium/Dewan",E_S_DESC="600 hingga 800 kerusi",E_S_FEE=1400.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Oditorium/Dewan",E_S_DESC="800 hingga 1,000 kerusi",E_S_FEE=1600.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Oditorium/Dewan",E_S_DESC="1,000 hingga 1,200 kerusi",E_S_FEE=1800.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Oditorium/Dewan",E_S_DESC="Lebih 1,200 kerusi",E_S_FEE=2000.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Oditorium/Dewan",E_S_DESC="Lesen Sementara",E_S_FEE=10.0f,E_S_PERIOD=4,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Panggung Wayang/Panggung",E_S_DESC="Tidak melebihi 200 kerusi",E_S_FEE=400.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Panggung Wayang/Panggung",E_S_DESC="Melebihi 200 kerusi tetapi tidak melebihi 400 kerusi",E_S_FEE=900.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Panggung Wayang/Panggung",E_S_DESC="Melebihi 400 kerusi tetapi tidak melebihi 600 kerusi",E_S_FEE=1100.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Panggung Wayang/Panggung",E_S_DESC="Melebihi 600 kerusi tetapi tidak melebihi 800 kerusi",E_S_FEE=1550.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Panggung Wayang/Panggung",E_S_DESC="Melebihi 800 kerusi tetapi tidak melebihi 1,000 kerusi",E_S_FEE=1800.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Panggung Wayang/Panggung",E_S_DESC="Melebihi 1,000 kerusi tetapi tidak melebihi 1,200 kerusi",E_S_FEE=2000.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Panggung Wayang/Panggung",E_S_DESC="Lebih 1,200 kerusi",E_S_FEE=2200.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Panggung Wayang/Panggung",E_S_DESC="Lesen Sementara",E_S_FEE=20.0f,E_S_PERIOD=4,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Pusat Hiburan (Dalam Bangunan)",E_S_DESC="Luas lantai tidak melebihi 30 meter persegi",E_S_FEE=300.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Pusat Hiburan (Dalam Bangunan)",E_S_DESC="Luas lantai melebihi 30 meter persegi tetapi tidak melebihi 60 meter persegi",E_S_FEE=500.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Pusat Hiburan (Dalam Bangunan)",E_S_DESC="Luas lantai melebihi 60 meter persegi tetapi tidak melebihi 90 meter persegi",E_S_FEE=800.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Pusat Hiburan (Dalam Bangunan)",E_S_DESC="Lebih 90 meter persegi",E_S_FEE=1100.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Pusat Hiburan/Taman Hiburan (Luar Bangunan)",E_S_DESC="Pusat Hiburan/Taman Hiburan (Luar Bangunan)",E_S_FEE=10.0f,E_S_PERIOD=4,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Lorong Boling",E_S_DESC="Lorong Boling",E_S_FEE=1100.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Gelanggang Luncur",E_S_DESC="Gelanggang Luncur",E_S_FEE=1000.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Mesin Hiburan (Kiddy Rides/Juke Box) di tempat selain daripada Pusat Hiburan",E_S_DESC="Mesin Hiburan (Kiddy Rides/Juke Box) di tempat selain daripada Pusat Hiburan",E_S_O_FEE=20.0f,E_S_O_NAME="mesin",E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Dewan Tarian/Disko/Kabaret",E_S_DESC="Luas lantai tidak melebihi 30 meter persegi",E_S_FEE=400.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Dewan Tarian/Disko/Kabaret",E_S_DESC="30 hingga 60 meter persegi",E_S_FEE=600.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Dewan Tarian/Disko/Kabaret",E_S_DESC="60 hingga 90 meter persegi",E_S_FEE=900.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Dewan Tarian/Disko/Kabaret",E_S_DESC="Lebih 90 meter persegi",E_S_FEE=1200.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Dewan Tarian/Disko/Kabaret",E_S_DESC="Kabaret - Lebih 90 meter persegi",E_S_FEE=2200.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Dewan Biliard/Snuker",E_S_DESC="Tidak melebihi 5 meja",E_S_FEE=500.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Dewan Biliard/Snuker",E_S_DESC="Melebihi 5 meja tetapi tidak melebihi 10 meja",E_S_FEE=1000.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Dewan Biliard/Snuker",E_S_DESC="Melebihi 10 meja tetapi tidak melebihi 20 meja",E_S_FEE=1500.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Dewan Biliard/Snuker",E_S_DESC="Melebihi 20 meja tetapi tidak melebihi 30 meja",E_S_FEE=2000.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Dewan Biliard/Snuker",E_S_DESC="Melebihi 30 meja tetapi tidak melebihi 40 meja",E_S_FEE=2500.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Dewan Biliard/Snuker",E_S_DESC="Melebihi 40 meja tetapi tidak melebihi 50 meja",E_S_FEE=3000.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Dewan Biliard/Snuker",E_S_DESC="Melebihi 50 meja",E_S_B_FEE=3000.0f,E_S_O_FEE=50.0f,E_S_O_NAME="meja",E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Stadium",E_S_DESC="Tidak melebihi 1000 kerusi",E_S_FEE=400.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Stadium",E_S_DESC="Melebihi 1000 kerusi",E_S_FEE=800.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Stadium",E_S_DESC="Lesen sementara",E_S_FEE=50.0f,E_S_PERIOD=4,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Ruang Legar/Dewan/Tempat Terbuka yang digunakan bagi pameran",E_S_DESC="Luas lantai tidak melebihi 100 meter persegi",E_S_FEE=100.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Ruang Legar/Dewan/Tempat Terbuka yang digunakan bagi pameran",E_S_DESC="Luas lantai melebihi 100 meter persegi tetapi tidak melebihi 150 meter persegi",E_S_FEE=200.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Ruang Legar/Dewan/Tempat Terbuka yang digunakan bagi pameran",E_S_DESC="Luas lantai melebihi 150 meter persegi tetapi tidak melebihi 200 meter persegi",E_S_FEE=300.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Ruang Legar/Dewan/Tempat Terbuka yang digunakan bagi pameran",E_S_DESC="Luas lantai melebihi 200 meter persegi",E_S_FEE=500.0f,E_S_PERIOD=1,E_S_P_QTY=1},
                new E_P_FEE {E_P_DESC="Ruang Legar/Dewan/Tempat Terbuka yang digunakan bagi pameran",E_S_DESC="Lesen sementara",E_S_FEE=10.0f,E_S_PERIOD=4,E_S_P_QTY=1},
            };
            entmtCodes.ForEach(s => context.E_P_FEEs.Add(s));
            context.SaveChanges();

            var departments = new List<DEPARTMENT>
            {
                new DEPARTMENT {DEP_CODE="Pelesenan",DEP_DESC="Bahagian Pelesenan",INTERNAL=1, ROUTE=0},
                new DEPARTMENT {DEP_CODE="ICT",DEP_DESC="Jabatan Pengurusan Maklumat",INTERNAL=1, ROUTE=0},
                new DEPARTMENT {DEP_CODE="BPP",DEP_DESC="Jabatan Perancangan & Kawalan Bangunan",INTERNAL=1, ROUTE=1},
                new DEPARTMENT {DEP_CODE="JPPPH",DEP_DESC="Jabatan Penilaian, Pelaburan dan Pengurusan Harta",INTERNAL=1, ROUTE=1},
                new DEPARTMENT {DEP_CODE="UKS",DEP_DESC="Unit Kesihatan",INTERNAL=1, ROUTE=1},
                new DEPARTMENT {DEP_CODE="PKPE",DEP_DESC="Pejabat Ketua Pegawai Eksekutif",INTERNAL=1, ROUTE=0},
                new DEPARTMENT {DEP_CODE="JBPM",DEP_DESC="Jabatan Bomba & Penyelamat Malaysia",INTERNAL=2, ROUTE=1},
                new DEPARTMENT {DEP_CODE="PDRM",DEP_DESC="Polis Diraja Malaysia",INTERNAL=2, ROUTE=1},
                new DEPARTMENT {DEP_CODE="JKDM",DEP_DESC="Jabatan Kastam Diraja Malaysia",INTERNAL=2, ROUTE=1},
            };
            departments.ForEach(s => context.DEPARTMENTs.Add(s));
            context.SaveChanges();

            var premisetypes = new List<PREMISETYPE>
            {
                new PREMISETYPE {PT_DESC="Hotel, Kompleks Perniagaan"},
                new PREMISETYPE {PT_DESC="Kompleks Pejabat"},
                new PREMISETYPE {PT_DESC="Rumah Kedai"},
                new PREMISETYPE {PT_DESC="Kedai Pejabat"},
                new PREMISETYPE {PT_DESC="Bangunan Kerajaan"},
            };
            premisetypes.ForEach(s => context.PREMISETYPEs.Add(s));
            context.SaveChanges();

            var individuals = new List<INDIVIDUAL>
            {
                new INDIVIDUAL{FULLNAME="Ali Bin Abu",MYKADNO="710213-12-4820",NAT_ID=1,PHONE="0108103140",ADD_IC="No.3, Kg. Tg. Aru, Jalan Tg. Aru, 87000 W.P.Labuan",IND_EMAIL="aliabu@yahoo.com",GENDER=1},
                new INDIVIDUAL{FULLNAME="Siti Aminah",MYKADNO="610122-12-4933",NAT_ID=1,PHONE="0112546778",ADD_IC="Lot 20, Blok F, Taman Mutiara, 87000 W.P.Labuan",IND_EMAIL="sitiaminah@gmail.com",GENDER=2},
                new INDIVIDUAL{FULLNAME="Chin Chee Kiong",MYKADNO="500101-12-5129",NAT_ID=1,PHONE="0148552370",ADD_IC="Lot 13, Blok D, Jalan Merdeka, Pusat Bandar, 87000 W.P.Labuan",IND_EMAIL="chinchee70@gmail.com",GENDER=1},
                new INDIVIDUAL{FULLNAME="Abdul Azis Hj Ibrahim",MYKADNO="600501125629",NAT_ID=1,GENDER=1},
                new INDIVIDUAL{FULLNAME="Arif Koh",MYKADNO="H0392480",NAT_ID=1,GENDER=1},
                new INDIVIDUAL{FULLNAME="Chan Chew Houi",MYKADNO="790402086273",NAT_ID=1,GENDER=1},
                new INDIVIDUAL{FULLNAME="Chua Kai Wen",MYKADNO="760814125411",NAT_ID=1,GENDER=1},
                new INDIVIDUAL{FULLNAME="Harilal Vasudevan",MYKADNO="660823125343",NAT_ID=1,GENDER=1},
                new INDIVIDUAL{FULLNAME="Hilary Koh Chin Kian @ Koh Chean Kan",MYKADNO="551109125597",NAT_ID=1,GENDER=1},
                new INDIVIDUAL{FULLNAME="Hj Mohd Ismail Bin Abdul Rahman",MYKADNO="540521125093",NAT_ID=1,GENDER=1},
                new INDIVIDUAL{FULLNAME="Imelda Binti Michael",MYKADNO="840110125552",NAT_ID=1,GENDER=2},
            };
            individuals.ForEach(s => context.INDIVIDUALs.Add(s));
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

            var AppStatus = new List<APPSTATUS>
            {
                new APPSTATUS {STATUSDESC="Draft created" ,PROGRESS =1},
                new APPSTATUS {STATUSDESC="Document Incomplete" ,PROGRESS =5},
                new APPSTATUS {STATUSDESC="Submitted to clerk" ,PROGRESS =10},
                new APPSTATUS {STATUSDESC="Processing by Clerk" ,PROGRESS =20},
                new APPSTATUS {STATUSDESC="Route Unit" ,PROGRESS =30},
                new APPSTATUS {STATUSDESC="Awaiting Director Response" ,PROGRESS =40},
                new APPSTATUS {STATUSDESC="Meeting" ,PROGRESS =50},
                new APPSTATUS {STATUSDESC="Scheduled For Meeting" ,PROGRESS =60},
                new APPSTATUS {STATUSDESC="KIV at Meeting" ,PROGRESS =65},
                new APPSTATUS {STATUSDESC="Awaiting CEO Approval" ,PROGRESS =60},
                new APPSTATUS {STATUSDESC="KIV at CEO" ,PROGRESS =60},
                new APPSTATUS {STATUSDESC="Letter of Notification (Approved)" ,PROGRESS =70},
                new APPSTATUS {STATUSDESC="Letter of Notification (Rejected)" ,PROGRESS =100},
                new APPSTATUS {STATUSDESC="Letter of Notification (Approved with Terms & Conditions)" ,PROGRESS =70},
                new APPSTATUS {STATUSDESC="Pending payment" ,PROGRESS =80},
                new APPSTATUS {STATUSDESC="Paid" ,PROGRESS =90},                
                new APPSTATUS {STATUSDESC="Complete" ,PROGRESS =100},
                new APPSTATUS {STATUSDESC="Express Draft" ,PROGRESS =1},
                new APPSTATUS {STATUSDESC="Express Pending Payment" ,PROGRESS =5},
                new APPSTATUS {STATUSDESC="Express Paid" ,PROGRESS =10},
                new APPSTATUS {STATUSDESC="Express Processing at Clerk" ,PROGRESS =15},
                new APPSTATUS {STATUSDESC="Express Route Unit" ,PROGRESS =25},
                new APPSTATUS {STATUSDESC="Express Meeting" ,PROGRESS =50},
                new APPSTATUS {STATUSDESC="Express Scheduled For Meeting" ,PROGRESS =60},
                new APPSTATUS {STATUSDESC="Express Letter of Compliance" ,PROGRESS =80},
                new APPSTATUS {STATUSDESC="Express Rejected" ,PROGRESS =100},
                new APPSTATUS {STATUSDESC="Express Approved" ,PROGRESS =100},
            };
            AppStatus.ForEach(s => context.APPSTATUS.Add(s));
            context.SaveChanges();

            var accesspages = new List<ACCESSPAGE>
            {
                new ACCESSPAGE {PAGEDESC="AccessPages",CRUDLEVEL=0,ROLEID=1,SCREENID=1},
                new ACCESSPAGE {PAGEDESC="AccessPages",CRUDLEVEL=0,ROLEID=2,SCREENID=1},
                new ACCESSPAGE {PAGEDESC="AccessPages",CRUDLEVEL=2,ROLEID=3,SCREENID=1},
                new ACCESSPAGE {PAGEDESC="AccessPages",CRUDLEVEL=3,ROLEID=4,SCREENID=1},
                new ACCESSPAGE {PAGEDESC="AccessPages",CRUDLEVEL=3,ROLEID=5,SCREENID=1},
                new ACCESSPAGE {PAGEDESC="AccessPages",CRUDLEVEL=4,ROLEID=6,SCREENID=1},
                new ACCESSPAGE {PAGEDESC="AccessPages",CRUDLEVEL=4,ROLEID=7,SCREENID=1},
                new ACCESSPAGE {PAGEDESC="AccessPages",CRUDLEVEL=4,ROLEID=8,SCREENID=1},

                new ACCESSPAGE {PAGEDESC="AdditionalInfos",CRUDLEVEL=0,ROLEID=1,SCREENID=2},
                new ACCESSPAGE {PAGEDESC="AdditionalInfos",CRUDLEVEL=0,ROLEID=2,SCREENID=2},
                new ACCESSPAGE {PAGEDESC="AdditionalInfos",CRUDLEVEL=2,ROLEID=3,SCREENID=2},
                new ACCESSPAGE {PAGEDESC="AdditionalInfos",CRUDLEVEL=3,ROLEID=4,SCREENID=2},
                new ACCESSPAGE {PAGEDESC="AdditionalInfos",CRUDLEVEL=3,ROLEID=5,SCREENID=2},
                new ACCESSPAGE {PAGEDESC="AdditionalInfos",CRUDLEVEL=4,ROLEID=6,SCREENID=2},
                new ACCESSPAGE {PAGEDESC="AdditionalInfos",CRUDLEVEL=4,ROLEID=7,SCREENID=2},
                new ACCESSPAGE {PAGEDESC="AdditionalInfos",CRUDLEVEL=4,ROLEID=8,SCREENID=2},

                new ACCESSPAGE {PAGEDESC="Attachment",CRUDLEVEL=0,ROLEID=1,SCREENID=3},
                new ACCESSPAGE {PAGEDESC="Attachment",CRUDLEVEL=0,ROLEID=2,SCREENID=3},
                new ACCESSPAGE {PAGEDESC="Attachment",CRUDLEVEL=2,ROLEID=3,SCREENID=3},
                new ACCESSPAGE {PAGEDESC="Attachment",CRUDLEVEL=3,ROLEID=4,SCREENID=3},
                new ACCESSPAGE {PAGEDESC="Attachment",CRUDLEVEL=3,ROLEID=5,SCREENID=3},
                new ACCESSPAGE {PAGEDESC="Attachment",CRUDLEVEL=4,ROLEID=6,SCREENID=3},
                new ACCESSPAGE {PAGEDESC="Attachment",CRUDLEVEL=4,ROLEID=7,SCREENID=3},
                new ACCESSPAGE {PAGEDESC="Attachment",CRUDLEVEL=4,ROLEID=8,SCREENID=3},

                new ACCESSPAGE {PAGEDESC="Administrator",CRUDLEVEL=0,ROLEID=1,SCREENID=4},
                new ACCESSPAGE {PAGEDESC="Administrator",CRUDLEVEL=0,ROLEID=2,SCREENID=4},
                new ACCESSPAGE {PAGEDESC="Administrator",CRUDLEVEL=2,ROLEID=3,SCREENID=4},
                new ACCESSPAGE {PAGEDESC="Administrator",CRUDLEVEL=3,ROLEID=4,SCREENID=4},
                new ACCESSPAGE {PAGEDESC="Administrator",CRUDLEVEL=3,ROLEID=5,SCREENID=4},
                new ACCESSPAGE {PAGEDESC="Administrator",CRUDLEVEL=4,ROLEID=6,SCREENID=4},
                new ACCESSPAGE {PAGEDESC="Administrator",CRUDLEVEL=4,ROLEID=7,SCREENID=4},
                new ACCESSPAGE {PAGEDESC="Administrator",CRUDLEVEL=4,ROLEID=8,SCREENID=4},

                new ACCESSPAGE {PAGEDESC="MasterSetup",CRUDLEVEL=0,ROLEID=1,SCREENID=5},
                new ACCESSPAGE {PAGEDESC="MasterSetup",CRUDLEVEL=0,ROLEID=2,SCREENID=5},
                new ACCESSPAGE {PAGEDESC="MasterSetup",CRUDLEVEL=2,ROLEID=3,SCREENID=5},
                new ACCESSPAGE {PAGEDESC="MasterSetup",CRUDLEVEL=3,ROLEID=4,SCREENID=5},
                new ACCESSPAGE {PAGEDESC="MasterSetup",CRUDLEVEL=3,ROLEID=5,SCREENID=5},
                new ACCESSPAGE {PAGEDESC="MasterSetup",CRUDLEVEL=4,ROLEID=6,SCREENID=5},
                new ACCESSPAGE {PAGEDESC="MasterSetup",CRUDLEVEL=4,ROLEID=7,SCREENID=5},
                new ACCESSPAGE {PAGEDESC="MasterSetup",CRUDLEVEL=4,ROLEID=8,SCREENID=5},

                new ACCESSPAGE {PAGEDESC="Inquiry",CRUDLEVEL=0,ROLEID=1,SCREENID=6},
                new ACCESSPAGE {PAGEDESC="Inquiry",CRUDLEVEL=0,ROLEID=2,SCREENID=6},
                new ACCESSPAGE {PAGEDESC="Inquiry",CRUDLEVEL=2,ROLEID=3,SCREENID=6},
                new ACCESSPAGE {PAGEDESC="Inquiry",CRUDLEVEL=3,ROLEID=4,SCREENID=6},
                new ACCESSPAGE {PAGEDESC="Inquiry",CRUDLEVEL=3,ROLEID=5,SCREENID=6},
                new ACCESSPAGE {PAGEDESC="Inquiry",CRUDLEVEL=4,ROLEID=6,SCREENID=6},
                new ACCESSPAGE {PAGEDESC="Inquiry",CRUDLEVEL=4,ROLEID=7,SCREENID=6},
                new ACCESSPAGE {PAGEDESC="Inquiry",CRUDLEVEL=4,ROLEID=8,SCREENID=6},

                new ACCESSPAGE {PAGEDESC="Reporting",CRUDLEVEL=0,ROLEID=1,SCREENID=7},
                new ACCESSPAGE {PAGEDESC="Reporting",CRUDLEVEL=1,ROLEID=2,SCREENID=7},
                new ACCESSPAGE {PAGEDESC="Reporting",CRUDLEVEL=2,ROLEID=3,SCREENID=7},
                new ACCESSPAGE {PAGEDESC="Reporting",CRUDLEVEL=3,ROLEID=4,SCREENID=7},
                new ACCESSPAGE {PAGEDESC="Reporting",CRUDLEVEL=3,ROLEID=5,SCREENID=7},
                new ACCESSPAGE {PAGEDESC="Reporting",CRUDLEVEL=4,ROLEID=6,SCREENID=7},
                new ACCESSPAGE {PAGEDESC="Reporting",CRUDLEVEL=4,ROLEID=7,SCREENID=7},
                new ACCESSPAGE {PAGEDESC="Reporting",CRUDLEVEL=4,ROLEID=8,SCREENID=7},

                new ACCESSPAGE {PAGEDESC="Individual",CRUDLEVEL=0,ROLEID=1,SCREENID=8},
                new ACCESSPAGE {PAGEDESC="Individual",CRUDLEVEL=3,ROLEID=2,SCREENID=8},
                new ACCESSPAGE {PAGEDESC="Individual",CRUDLEVEL=3,ROLEID=3,SCREENID=8},
                new ACCESSPAGE {PAGEDESC="Individual",CRUDLEVEL=3,ROLEID=4,SCREENID=8},
                new ACCESSPAGE {PAGEDESC="Individual",CRUDLEVEL=3,ROLEID=5,SCREENID=8},
                new ACCESSPAGE {PAGEDESC="Individual",CRUDLEVEL=4,ROLEID=6,SCREENID=8},
                new ACCESSPAGE {PAGEDESC="Individual",CRUDLEVEL=4,ROLEID=7,SCREENID=8},
                new ACCESSPAGE {PAGEDESC="Individual",CRUDLEVEL=4,ROLEID=8,SCREENID=8},

                new ACCESSPAGE {PAGEDESC="DeskOfficer",CRUDLEVEL=0,ROLEID=1,SCREENID=9},
                new ACCESSPAGE {PAGEDESC="DeskOfficer",CRUDLEVEL=1,ROLEID=2,SCREENID=9},
                new ACCESSPAGE {PAGEDESC="DeskOfficer",CRUDLEVEL=0,ROLEID=3,SCREENID=9},
                new ACCESSPAGE {PAGEDESC="DeskOfficer",CRUDLEVEL=0,ROLEID=4,SCREENID=9},
                new ACCESSPAGE {PAGEDESC="DeskOfficer",CRUDLEVEL=0,ROLEID=5,SCREENID=9},
                new ACCESSPAGE {PAGEDESC="DeskOfficer",CRUDLEVEL=0,ROLEID=6,SCREENID=9},
                new ACCESSPAGE {PAGEDESC="DeskOfficer",CRUDLEVEL=0,ROLEID=7,SCREENID=9},
                new ACCESSPAGE {PAGEDESC="DeskOfficer",CRUDLEVEL=0,ROLEID=8,SCREENID=9},

                new ACCESSPAGE {PAGEDESC="Profile",CRUDLEVEL=0,ROLEID=1,SCREENID=10},
                new ACCESSPAGE {PAGEDESC="Profile",CRUDLEVEL=2,ROLEID=2,SCREENID=10},
                new ACCESSPAGE {PAGEDESC="Profile",CRUDLEVEL=2,ROLEID=3,SCREENID=10},
                new ACCESSPAGE {PAGEDESC="Profile",CRUDLEVEL=3,ROLEID=4,SCREENID=10},
                new ACCESSPAGE {PAGEDESC="Profile",CRUDLEVEL=3,ROLEID=5,SCREENID=10},
                new ACCESSPAGE {PAGEDESC="Profile",CRUDLEVEL=4,ROLEID=6,SCREENID=10},
                new ACCESSPAGE {PAGEDESC="Profile",CRUDLEVEL=4,ROLEID=7,SCREENID=10},
                new ACCESSPAGE {PAGEDESC="Profile",CRUDLEVEL=4,ROLEID=8,SCREENID=10},

                new ACCESSPAGE {PAGEDESC="Process",CRUDLEVEL=0,ROLEID=1,SCREENID=11},
                new ACCESSPAGE {PAGEDESC="Process",CRUDLEVEL=1,ROLEID=2,SCREENID=11},
                new ACCESSPAGE {PAGEDESC="Process",CRUDLEVEL=2,ROLEID=3,SCREENID=11},
                new ACCESSPAGE {PAGEDESC="Process",CRUDLEVEL=3,ROLEID=4,SCREENID=11},
                new ACCESSPAGE {PAGEDESC="Process",CRUDLEVEL=3,ROLEID=5,SCREENID=11},
                new ACCESSPAGE {PAGEDESC="Process",CRUDLEVEL=4,ROLEID=6,SCREENID=11},
                new ACCESSPAGE {PAGEDESC="Process",CRUDLEVEL=4,ROLEID=7,SCREENID=11},
                new ACCESSPAGE {PAGEDESC="Process",CRUDLEVEL=4,ROLEID=8,SCREENID=11},

                new ACCESSPAGE {PAGEDESC="Company",CRUDLEVEL=1,ROLEID=1,SCREENID=12},
                new ACCESSPAGE {PAGEDESC="Company",CRUDLEVEL=4,ROLEID=2,SCREENID=12},
                new ACCESSPAGE {PAGEDESC="Company",CRUDLEVEL=4,ROLEID=3,SCREENID=12},
                new ACCESSPAGE {PAGEDESC="Company",CRUDLEVEL=4,ROLEID=4,SCREENID=12},
                new ACCESSPAGE {PAGEDESC="Company",CRUDLEVEL=1,ROLEID=5,SCREENID=12},
                new ACCESSPAGE {PAGEDESC="Company",CRUDLEVEL=4,ROLEID=6,SCREENID=12},
                new ACCESSPAGE {PAGEDESC="Company",CRUDLEVEL=4,ROLEID=7,SCREENID=12},
                new ACCESSPAGE {PAGEDESC="Company",CRUDLEVEL=4,ROLEID=8,SCREENID=12},

                new ACCESSPAGE {PAGEDESC="Application",CRUDLEVEL=2,ROLEID=1,SCREENID=13},
                new ACCESSPAGE {PAGEDESC="Application",CRUDLEVEL=3,ROLEID=2,SCREENID=13},
                new ACCESSPAGE {PAGEDESC="Application",CRUDLEVEL=3,ROLEID=3,SCREENID=13},
                new ACCESSPAGE {PAGEDESC="Application",CRUDLEVEL=4,ROLEID=4,SCREENID=13},
                new ACCESSPAGE {PAGEDESC="Application",CRUDLEVEL=3,ROLEID=5,SCREENID=13},
                new ACCESSPAGE {PAGEDESC="Application",CRUDLEVEL=3,ROLEID=6,SCREENID=13},
                new ACCESSPAGE {PAGEDESC="Application",CRUDLEVEL=3,ROLEID=7,SCREENID=13},
                new ACCESSPAGE {PAGEDESC="Application",CRUDLEVEL=4,ROLEID=8,SCREENID=13},

                new ACCESSPAGE {PAGEDESC="Meeting",CRUDLEVEL=0,ROLEID=1,SCREENID=14},
                new ACCESSPAGE {PAGEDESC="Meeting",CRUDLEVEL=1,ROLEID=2,SCREENID=14},
                new ACCESSPAGE {PAGEDESC="Meeting",CRUDLEVEL=1,ROLEID=3,SCREENID=14},
                new ACCESSPAGE {PAGEDESC="Meeting",CRUDLEVEL=4,ROLEID=4,SCREENID=14},
                new ACCESSPAGE {PAGEDESC="Meeting",CRUDLEVEL=0,ROLEID=5,SCREENID=14},
                new ACCESSPAGE {PAGEDESC="Meeting",CRUDLEVEL=4,ROLEID=6,SCREENID=14},
                new ACCESSPAGE {PAGEDESC="Meeting",CRUDLEVEL=1,ROLEID=7,SCREENID=14},
                new ACCESSPAGE {PAGEDESC="Meeting",CRUDLEVEL=1,ROLEID=8,SCREENID=14},
            };
            accesspages.ForEach(s => context.ACCESSPAGEs.Add(s));
            context.SaveChanges();

            var users = new List<USERS>
            {
                new USERS {FULLNAME="Abd Aziz Bin Hamzah",USERNAME="aziz",PASSWORD="rGWQ/rZGq74=",EMAIL="aziz.h@pl.gov.my", ROLEID=6,DEP_ID=1},
                new USERS {FULLNAME="Soffiyan Bin Hadis",USERNAME="soffiyan",PASSWORD="rGWQ/rZGq74=",EMAIL="soffiyan.hadis@pl.gov.my", ROLEID=4,DEP_ID=1},
                new USERS {FULLNAME="Hjh. Simai Binti Md Jamil",USERNAME="simai",PASSWORD="rGWQ/rZGq74=",EMAIL="simai@pl.gov.my", ROLEID=3,DEP_ID=1},
                new USERS {FULLNAME="Suwardi Binti Muali",USERNAME="suwardi",PASSWORD="rGWQ/rZGq74=",EMAIL="suwardi.muali.pl@1govuc.gov.my", ROLEID=2,DEP_ID=1},
                new USERS {FULLNAME="Adey Suhaimi Bin Suhaili",USERNAME="adey",PASSWORD="rGWQ/rZGq74=",EMAIL="adey.suhaimi.pl@1govuc.gov.my", ROLEID=3,DEP_ID=1},
                new USERS {FULLNAME="Azean Irdawati Binti Wahid",USERNAME="azean",PASSWORD="rGWQ/rZGq74=",EMAIL="azean.wahid.pl@1govuc.gov.my", ROLEID=3,DEP_ID=1},
                new USERS {FULLNAME="Kazlina Binti Kassim",USERNAME="kazlina",PASSWORD="rGWQ/rZGq74=",EMAIL="kazlina@yahoo.com", ROLEID=3,DEP_ID=1},
                new USERS {FULLNAME="Johaniza Binti Jonait",USERNAME="johaniza",PASSWORD="rGWQ/rZGq74=",EMAIL="johaniza@yahoo.com", ROLEID=3,DEP_ID=1},
                new USERS {FULLNAME="Rafidah Binti Mohd Isa",USERNAME="rafidah",PASSWORD="rGWQ/rZGq74=",EMAIL="rafidah@yahoo.com", ROLEID=2,DEP_ID=1},
                new USERS {FULLNAME="Ahmad Jais Bin Halon",USERNAME="ahmadjais",PASSWORD="rGWQ/rZGq74=",EMAIL="ahmad.jais@yahoo.com", ROLEID=2,DEP_ID=1},
                new USERS {FULLNAME="YBHG. Datuk Azhar Bin Ahmad",USERNAME="kpe",PASSWORD="rGWQ/rZGq74=",EMAIL="azharahmad@pl.gov.my", ROLEID=7,DEP_ID=1},
                new USERS {FULLNAME="Mazalan Bin Hassin",USERNAME="mazalan",PASSWORD="rGWQ/rZGq74=",EMAIL="mazalan.hassin@pl.gov.my", ROLEID=8,DEP_ID=1},
                new USERS {FULLNAME="R. Norasliana Binti Ramlee",USERNAME="norasliana",PASSWORD="rGWQ/rZGq74=",EMAIL="ana.ramli@pl.gov.my", ROLEID=8,DEP_ID=1},
                new USERS {FULLNAME="Jabatan Bomba & Penyelamat Malaysia",USERNAME="bomba",PASSWORD="rGWQ/rZGq74=",EMAIL="jbpm_labuan.bomba@1govuc.gov.my", ROLEID=5,DEP_ID=1},
                new USERS {FULLNAME="Wakil Bahagian Perancangan",USERNAME="bpp",PASSWORD="rGWQ/rZGq74=",EMAIL="bpp@pl.gov.my", ROLEID=5,DEP_ID=3},
                new USERS {FULLNAME="Wakil Unit Kesihatan",USERNAME="uks",PASSWORD="rGWQ/rZGq74=",EMAIL="uks@pl.gov.my", ROLEID=5,DEP_ID=5},
                new USERS {FULLNAME="Wakil Jabatan Penilaian",USERNAME="jppph",PASSWORD="rGWQ/rZGq74=",EMAIL="jppph@pl.gov.my", ROLEID=5,DEP_ID=4},
                new USERS {FULLNAME="Ronny Jimmy",USERNAME="ronny",PASSWORD="rGWQ/rZGq74=",EMAIL="ronnyrtg@yahoo.com", ROLEID=8,DEP_ID=1},
            };
            users.ForEach(s => context.USERS.Add(s));
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
                new RD {RD_DESC = "Salinan Sijil Suntikan TY2 (Pengendalian Makanan)(jika ada)" },
                new RD {RD_DESC = "Salinan Sijil Kursus Pengendalian Makanan" },
                new RD {RD_DESC = "Sijil Pendaftaran Premis Makanan daripada KKM" },
                new RD {RD_DESC = "Salinan Resit (bukti) pembelian alat penapis minyak" },
                new RD {RD_DESC = "(Salun rambut, Gunting Rambut, SPA, Refleksologi dll) Pengesahan kesihatan bagi pengusaha dan pekerja daripada pengamal perubatan yang diiktiraf (jika ada)" },
                new RD {RD_DESC = "Surat sokongan Bahagian Perkhidmatan Farmasi dan Jabatan Kesihatan Negeri" },
                new RD {RD_DESC = "Salinan Sijil Pendaftaran daripada Jabatan Pelajaran Negeri/Kementerian Pendidikan bagi TADIKA/PUSAT TUISYEN/PUSAT PENGAJIAN" },
                new RD {RD_DESC = "Salinan Perakuan Pendaftaran sementara daripada Jabatan Kebajikan Masyarakat (JKM) bagi TASKA/PUSAT JAGAAN" },
                new RD {RD_DESC = "Salinan Sijil Suntikan TY2 (Pengendalian Makanan) bagi premis yang menyediakan makanan (jika berkaitan)" },
                new RD {RD_DESC = "Perakuan Mesin Jentera daripada Jabatan Keselamatan & Kesihatan Pekerjaan (JKKP) (jika berkaitan)" },
                new RD {RD_DESC = "Kelulusan daripada Jabatan Alam Sekitar (bagi aktiviti yang menghasilkan sisa buangan yang membahayakan persekitaran)" },
                new RD {RD_DESC = "Kelulusan daripada Jabatan Bomba dan Penyelamat Malaysia" },
                new RD {RD_DESC = "Tambahan dokumen sokongan mengikut aktiviti perkilangan seperti lampiran II (jika berkaitan)" },
                new RD {RD_DESC = "Salinan visual ilustrasi iklan yang diluluskan oleh DBP dan mengikut spesifikasi yang ditetapkan PBT (memastikan penggunaan Bahasa Kebangsaan lebih utama & lebih besar dari bahasa lain di Malaysia/penggunaan Bahasa asing tidak dibenarkan" },
                new RD {RD_DESC = "Gambar lokasi@kedudukan tempat pemasangan iklan premis" },
                new RD {RD_DESC = "Salinan Kad Pengenalan" },
                new RD {RD_DESC = "Gambar berukuran pasport (2 keping)" },
                new RD {RD_DESC = "Salinan Pendaftaran Perniagaan (ROC/ROB/SSM)" },
                new RD {RD_DESC = "Gambar dan pelan lokasi tapak menjaja (penjaja statik)" },
                new RD {RD_DESC = "Salinan Geran Pendaftaran Kenderaan/Kelulusan JPJ/Surat kebenaran pemilik kenderaan jika bukan kenderaan milik sendiri/lesen memandu (yang melibatkan kenderaan sahaja)" },
                new RD {RD_DESC = "Surat Tawaran/Pengesahan Penganjur Pihak Berkuasa Tempatan *jika berkenaan" },
                new RD {RD_DESC = "Perakuan Pendaftaran daripada Jabatan Pertanian" },
                new RD {RD_DESC = "Lesen runcit barangan terkawal daripada KPDNKK" },
                new RD {RD_DESC = "Lesen Mengilang bagi Barang Kawalan Berjadual Di bawah Akta Kawalan Bekalan 1961 (CSA)" },
                new RD {RD_DESC = "Lesen Membuat/Membaiki/Menjual Alat-alat Timbang & Sukat Di bawah Akta Timbang & Sukat 1972" },
                new RD {RD_DESC = "Kelulusan untuk Mengilang Papan Suis" },
                new RD {RD_DESC = "Mengilang Peralatan Gas" },
                new RD {RD_DESC = "Lesen Biobahan Api" },
                new RD {RD_DESC = "Lesen Membina Kilang" },
                new RD {RD_DESC = "Lesen Pekebun Kecil" },
                new RD {RD_DESC = "Lesen Estet" },
                new RD {RD_DESC = "Lesen Mengawet Tembakau" },
                new RD {RD_DESC = "Lesen Mengilang Tembakau atau Keluaran Tembakau" },
                new RD {RD_DESC = "Lesen Pembuatan Produk Getah" },
                new RD {RD_DESC = "Sijil Pengiktirafan Pendaftaran Bengkel Kejuruteraan (Bina Badan/Pengubahsuaian Teknikal Kenderaan Perdagangan)" },
                new RD {RD_DESC = "Kelulusan Pendaftaran Kilang Kenderaan Perdagangan Bina Semula (Rebuilt)" },
                new RD {RD_DESC = "Pendaftaran Bengkel Kejuruteraan (pemasangan dan pengubahsuaian teknikal bagi CNG/NGV)" },
                new RD {RD_DESC = "Lesen bagi menduduki dan menggunakan Premis yang ditetapkan (Minyak Kelapa Sawit Mentah" },
                new RD {RD_DESC = "Lesen bagi menduduki dan menggunakan Premis yang ditetapkan (Getah Asli Mentah)" },
                new RD {RD_DESC = "Lesen Industri berasaskan kayu (Kilang Papan, Plywood, Moulding, Perabot dll)" },
                new RD {RD_DESC = "Lesen Pengilang Keluaran Berdaftar (Farmaseutikal)" },
                new RD {RD_DESC = "Skim Pensijilan Good Manufacturing Practice (GMP), HACCP" },
                new RD {RD_DESC = "Lesen Kilang Padi Komersial" },
                new RD {RD_DESC = "Sijil Pendaftaran Pembuat Makanan Haiwan atau Bahan Tambahan Makanan Haiwan" },
                new RD {RD_DESC = "Pendaftaran Kilang Nanas dan Kilang Kecil" },
                new RD {RD_DESC = "Permit mengimport/membeli/memakai/memiliki/mengilang bagi membuat dan menjual baju kalis peluru" },
                new RD {RD_DESC = "Lesen Senjata Api" },
                new RD {RD_DESC = "Lesen Cakera Optik di bawah Akta Cakera Optik 2000" },
                new RD {RD_DESC = "kelulusan Jawatankuasa Perdagangan Pengedaran (bagi pemilik bukan warganegara sahaja)" },
                new RD {RD_DESC = "Lesen Pedagang Koko" },
                new RD {RD_DESC = "Lesen Peniaga/Taxidermi" },
                new RD {RD_DESC = "Lesen Runcit Beras" },
                new RD {RD_DESC = "Lesen Akta Jualan Langsung" },
                new RD {RD_DESC = "Permohonan membawa masuk dan penjualan 'pepper spray/security spray'" },
                new RD {RD_DESC = "Lesen Magazin Bahan Letupan" },
                new RD {RD_DESC = "Lesen daripada FINAS" },
                new RD {RD_DESC = "Perakuan Pendaftaran Farmasi" },
                new RD {RD_DESC = "Perakuan Kelayakan Optometri" },
                new RD {RD_DESC = "Permit berniaga Peralatan dan Pakaian Seragam Polis" },
                new RD {RD_DESC = "Lesen Perniagaan Pengendalian Pelancongan dan Perniagaan Agensi Pengembaraan" },
                new RD {RD_DESC = "Pendaftaran Syarikat Perunding dan Jurutera Profesional" },
                new RD {RD_DESC = "Lesen Pemaju Perumahan Permit Iklan & Jualan Baru Pemaju" },
                new RD {RD_DESC = "Lesen Agensi Pekerjaan Swasta" },
                new RD {RD_DESC = "Lesen Agensi Persendirian" },
                new RD {RD_DESC = "Pendaftaran Syarikat Amalan Perunding Kejuruteraan" },
                new RD {RD_DESC = "Sijil Perakuan Pendaftaran Kontraktor" },
                new RD {RD_DESC = "Kelulusan Jabatan Pendidikan Negeri" },
                new RD {RD_DESC = "Pendaftaran IPTS" },
                new RD {RD_DESC = "Permit Sekolah/Institut Memandu" },
                new RD {RD_DESC = "Perakuan Kelulusan Sekolah Latihan Penerbangan" },
                new RD {RD_DESC = "Lesen Institut Latihan Pelancongan" },
                new RD {RD_DESC = "Pengiktirafan Sekolah Latihan Pengendali Makanan (SLPM)" },
                new RD {RD_DESC = "Kelulusan Jabatan Pendidikan Negeri" },
                new RD {RD_DESC = "Perakuan Pendaftaran Taman Asuhan Kanak-kanak" },
                new RD {RD_DESC = "Perakuan Pendaftaran Pusat Jagaan" },
                new RD {RD_DESC = "Pendaftaran Hotel" },
                new RD {RD_DESC = "Pendaftaran Premis Penginapan Pelancong" },
                new RD {RD_DESC = "Perakuan Pendaftaran untuk Menubuhkan/Menyenggarakan/Mengendalikan/Menyediakan Klinik Perubatan/Pergigian Swasta" },
                new RD {RD_DESC = "Perakuan Kelulusan untuk Menubuhkan/Menyenggarakan Kemudahan/Perkhidmatan Jagaan Kesihatan Swasta" },
                new RD {RD_DESC = "Lesen untuk Mengendalikan/Menyediakan Kemudahan/Perkhidmatan Jagaan Kesihatan Swasta" },
                new RD {RD_DESC = "Pendaftaran Penuh Optometris" },
                new RD {RD_DESC = "Sijil Amanah Tahunan" },
                new RD {RD_DESC = "Sijil Pendaftaran Pertubuhan Perbadanan" },
                new RD {RD_DESC = "Perakuan Kelayakan Optometri" },
                new RD {RD_DESC = "Perakuan Pendaftaran Pengamal Pergigian" },
                new RD {RD_DESC = "Kebenaran Di Bawah Petroleum Development Act 1974 (PDA 1,2,3 dan 4)" },
                new RD {RD_DESC = "Lesen Menjual Barang-barang Lusuh" },
                new RD {RD_DESC = "Lesen Menyimpan Hidupan Liar yang Dilindungi" },
                new RD {RD_DESC = "Lesen Penggudangan" },
                new RD {RD_DESC = "Lesen Penggurup Wang" },
                new RD {RD_DESC = "Lesen Pembrokeran Wang" },
                new RD {RD_DESC = "Lesen Perniagaan Perkhidmatan Wang" },
                new RD {RD_DESC = "lesen Insurans" },
                new RD {RD_DESC = "Lesen Mengimport Rokok dan Minuman Keras" },
                new RD {RD_DESC = "Lesen Perfileman Malaysia" },
                new RD {RD_DESC = "Lesen Borong Beras" },
                new RD {RD_DESC = "Lesen Control Supply Act (CSA)" },
                new RD {RD_DESC = "Lesen Pengisar" },
                new RD {RD_DESC = "Lesen Tapak Semaian Getah (Nurseri)" },
                new RD {RD_DESC = "Lesen Mesin Cetak" },
                new RD {RD_DESC = "Lesen Pemberi Pinjam Wang" },
                new RD {RD_DESC = "lesen Pemegang Pajak Gadai" },
                new RD {RD_DESC = "Surat Kebenaran Agen Penghantaran" },
                new RD {RD_DESC = "Lesen Perkhidmatan Kurier" },
                new RD {RD_DESC = "Lesen Rumah Sembelih Swasta" },
                new RD {RD_DESC = "Pendaftaran Syarikat Filem" },
                new RD {RD_DESC = "Lesen Pengendali Lori" },
                new RD {RD_DESC = "Lesen Pengendali Bas" },
                new RD {RD_DESC = "Lesen Perusahaan atau Penyediaan Perkhidmatan Pengurusan Pembersihan Awam" },
                new RD {RD_DESC = "Lesen Perusahaan atau Penyediaan Perkhidmatan Pemungutan bagi Sisa Pepejal Isi Rumah, Sisa Pepejal Awam, Sisa Pepejal Keinstitusian Awam dan Sisa Pepejal yang Serupa" },
                new RD {RD_DESC = "Lesen Perkhidmatan Pengangkutan dengan Long Haulage" },
                new RD {RD_DESC = "Lesen Judi" },

            };
            rds.ForEach(s => context.RDs.Add(s));
            context.SaveChanges();

            var companies = new List<COMPANY>
            {
                new COMPANY {REG_NO="75278-T",C_NAME="Chin Recycle",C_PHONE="087430010" },
                new COMPANY {REG_NO="801234-V",C_NAME="Kejora Bersatu Sdn Bhd",C_PHONE="087450690" },
                new COMPANY {REG_NO="991345-V",C_NAME="Kentucky Fried Chicken (KFC)",C_PHONE="087421090" },
                new COMPANY {REG_NO="129074-M",C_NAME="Marry Brown",C_PHONE="087-411555" },
                new COMPANY {REG_NO="203976-T",C_NAME="Borneo Combat Gym",C_PHONE="011-3516 1698" },
                new COMPANY {REG_NO="987264-H",C_NAME="Dorsett Grand Labuan",C_PHONE="+608 7422 000" },
                new COMPANY {REG_NO="987264-H",C_NAME="Red Tomato Hotel",C_PHONE="087-412 963" },
                new COMPANY {REG_NO="355817-T",C_NAME="Olympic Pool & Snooker",C_PHONE="087-467522" },
                new COMPANY {REG_NO="188846-T",C_NAME="Kedai Gunting Rambut Wahab" },
                new COMPANY {REG_NO="203433-V",C_NAME="Klinik Suria (Labuan) Sdn. Bhd.",C_PHONE="087-504 969" },
                new COMPANY {REG_NO="203433-V",C_NAME="Wong Brothers Workshop & Service Sdn. Bhd.",C_PHONE="087-414 784" },
                new COMPANY {REG_NO="203433-V",C_NAME="Hobby Mix.",C_PHONE="087-429 428" },
                new COMPANY {C_NAME="Thirumurugan Temple"},
                new COMPANY {C_NAME="Jabatan Kerja Raya",C_PHONE="087-414 040"},
                new COMPANY {C_NAME="Sekolah Menengah Sains Labuan",C_PHONE="(+60) 87 461525"},
            };
            companies.ForEach(s => context.COMPANIES.Add(s));
            context.SaveChanges();

            var indlinkcoms = new List<IND_L_COM>
            {
                new IND_L_COM {IND_ID=1,COMPANYID=2 },
                new IND_L_COM {IND_ID=2,COMPANYID=2 },
                new IND_L_COM {IND_ID=3,COMPANYID=1 },
            };
            indlinkcoms.ForEach(s => context.IND_L_COMs.Add(s));
            context.SaveChanges();

            var zones = new List<ZONE_M>
            {
                new ZONE_M {ZONE_CODE="11",ZONE_DESC="Bandar-Perdagangan"},
                new ZONE_M {ZONE_CODE="12",ZONE_DESC="Bandar-Perindustrian"},
                new ZONE_M {ZONE_CODE="13",ZONE_DESC="Bandar-Tanah Kosong"},
                new ZONE_M {ZONE_CODE="14",ZONE_DESC="Bandar-Perumahan dalam taman perumahan"},
                new ZONE_M {ZONE_CODE="15",ZONE_DESC="Bandar-Perumahan di luar taman perumahan"},
                new ZONE_M {ZONE_CODE="1A",ZONE_DESC="SMK-Perdagangan"},
                new ZONE_M {ZONE_CODE="1B",ZONE_DESC="SMK-Perindustrian"},
                new ZONE_M {ZONE_CODE="1D",ZONE_DESC="SMK-Tanah Kosong"},
                new ZONE_M {ZONE_CODE="1E",ZONE_DESC="SMK-Perumahan dalam taman perumahan"},
                new ZONE_M {ZONE_CODE="1F",ZONE_DESC="SMK-Perumahan di luar taman perumahan"},
            };
            zones.ForEach(s => context.ZONEs.Add(s));
            context.SaveChanges();

            var locations = new List<LOCATION>
            {
                new LOCATION {L_CODE="01",L_DESC="Jalan Tun Mustapha"},
                new LOCATION {L_CODE="02",L_DESC="Jalan Dewan"},
                new LOCATION {L_CODE="03",L_DESC="Jalan Merdeka"},
                new LOCATION {L_CODE="04",L_DESC="Jalan Bahasa"},
                new LOCATION {L_CODE="05",L_DESC="Jalan Bunga Kenanga"},
                new LOCATION {L_CODE="06",L_DESC="Jalan Bunga Raya"},
                new LOCATION {L_CODE="07",L_DESC="Jalan Perpaduan"},
                new LOCATION {L_CODE="08",L_DESC="Jalan Bunga Tanjung"},
                new LOCATION {L_CODE="09",L_DESC="Jalan OKK Awang Besar"},
                new LOCATION {L_CODE="10",L_DESC="Jalan Muhibah"},
            };
            locations.ForEach(s => context.LOCATIONs.Add(s));
            context.SaveChanges();

            var roads = new List<ROAD_M>
            {
                new ROAD_M {ROAD_CODE="001",ROAD_DESC="Jalan Tun Mustapha"},
                new ROAD_M {ROAD_CODE="002",ROAD_DESC="Jalan Dewan"},
                new ROAD_M {ROAD_CODE="003",ROAD_DESC="Jalan Merdeka"},
                new ROAD_M {ROAD_CODE="004",ROAD_DESC="Jalan Bahasa"},
                new ROAD_M {ROAD_CODE="005",ROAD_DESC="Jalan Bunga Kenanga"},
                new ROAD_M {ROAD_CODE="006",ROAD_DESC="Jalan Bunga Raya"},
                new ROAD_M {ROAD_CODE="007",ROAD_DESC="Jalan Perpaduan"},
                new ROAD_M {ROAD_CODE="008",ROAD_DESC="Jalan Bunga Tanjung"},
                new ROAD_M {ROAD_CODE="009",ROAD_DESC="Jalan OKK Awang Besar"},
                new ROAD_M {ROAD_CODE="010",ROAD_DESC="Jalan Tanjung Pasir"},
            };
            roads.ForEach(s => context.ROADs.Add(s));
            context.SaveChanges();

            var btls = new List<RD_L_BT>
            {
                new RD_L_BT {BT_ID=1,RD_ID=13},
                new RD_L_BT {BT_ID=1,RD_ID=41},
                new RD_L_BT {BT_ID=1,RD_ID=2},
                new RD_L_BT {BT_ID=2,RD_ID=1},
                new RD_L_BT {BT_ID=2,RD_ID=2},
                new RD_L_BT {BT_ID=2,RD_ID=3},
                new RD_L_BT {BT_ID=3,RD_ID=1},
                new RD_L_BT {BT_ID=3,RD_ID=2},
                new RD_L_BT {BT_ID=3,RD_ID=3},
                new RD_L_BT {BT_ID=4,RD_ID=1},
                new RD_L_BT {BT_ID=4,RD_ID=2},
                new RD_L_BT {BT_ID=4,RD_ID=3},
                new RD_L_BT {BT_ID=5,RD_ID=1},
                new RD_L_BT {BT_ID=5,RD_ID=2},
                new RD_L_BT {BT_ID=5,RD_ID=3},
                new RD_L_BT {BT_ID=6,RD_ID=1},
                new RD_L_BT {BT_ID=6,RD_ID=2},
                new RD_L_BT {BT_ID=6,RD_ID=3},
            };
            btls.ForEach(s => context.RD_L_BTs.Add(s));
            context.SaveChanges();

            var bclinkads = new List<RD_L_BC>
            {
                new RD_L_BC {BC_ID=2,RD_ID=3},
                new RD_L_BC {BC_ID=2,RD_ID=4},
            };
            bclinkads.ForEach(s => context.RD_L_BCs.Add(s));
            context.SaveChanges();

            var rdlinklt = new List<RD_L_LT>
            {
                new RD_L_LT {LIC_TYPEID=1,RD_ID=1},
                new RD_L_LT {LIC_TYPEID=1,RD_ID=4},
                new RD_L_LT {LIC_TYPEID=5,RD_ID=14},
                new RD_L_LT {LIC_TYPEID=5,RD_ID=37},
            };
            rdlinklt.ForEach(s => context.RD_L_LTs.Add(s));
            context.SaveChanges();


        }
    }
}
