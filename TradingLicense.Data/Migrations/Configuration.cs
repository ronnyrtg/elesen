namespace TradingLicense.Data.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
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
                new Sector {SectorID=7,SectorDesc="Petempatan Makanan"},
                new Sector {SectorID=8,SectorDesc="Hotel dan Rumah Tumpangan"},
                new Sector {SectorID=9,SectorDesc="Pengurusan Skrap"},
            };
            sector.ForEach(s => context.Sectors.Add(s));
            context.SaveChanges();

            var entmtGroup = new List<EntmtGroup>
            {
                new EntmtGroup {EntmtGroupID=1,EntmtGroupCode="L001",EntmtGroupDesc="Oditorium/Dewan"},
                new EntmtGroup {EntmtGroupID=2,EntmtGroupCode="L002",EntmtGroupDesc="Panggung Wayang/Panggung"},
                new EntmtGroup {EntmtGroupID=3,EntmtGroupCode="L003",EntmtGroupDesc="Pusat Hiburan (Dalam Bangunan)"},
                new EntmtGroup {EntmtGroupID=4,EntmtGroupCode="L004",EntmtGroupDesc="Pusat Hiburan/Taman Hiburan (Luar Bangunan)"},
                new EntmtGroup {EntmtGroupID=5,EntmtGroupCode="L005",EntmtGroupDesc="Lorong Boling"},
                new EntmtGroup {EntmtGroupID=6,EntmtGroupCode="L006",EntmtGroupDesc="Gelanggang Luncur"},
                new EntmtGroup {EntmtGroupID=7,EntmtGroupCode="L007",EntmtGroupDesc="Mesin Hiburan (Kiddy Rides/Juke Box) di tempat selain daripada Pusat Hiburan"},
                new EntmtGroup {EntmtGroupID=8,EntmtGroupCode="L008",EntmtGroupDesc="Dewan Tarian/Disko/Kabaret"},
                new EntmtGroup {EntmtGroupID=9,EntmtGroupCode="L009",EntmtGroupDesc="Dewan Biliard/Snuker"},
                new EntmtGroup {EntmtGroupID=10,EntmtGroupCode="L010",EntmtGroupDesc="Stadium"},
                new EntmtGroup {EntmtGroupID=11,EntmtGroupCode="L011",EntmtGroupDesc="Ruang Legar/Dewan/Tempat Terbuka yang digunakan bagi pameran"},
                new EntmtGroup {EntmtGroupID=12,EntmtGroupCode="L012",EntmtGroupDesc="Hiburan bagi maksud pendidikan yang disediakan oleh sekolah, universiti, maktab, PIBG, kumpulan guru atau murid"},
                new EntmtGroup {EntmtGroupID=13,EntmtGroupCode="L013",EntmtGroupDesc="Sukan atau permainan bertaraf amatur"},
                new EntmtGroup {EntmtGroupID=14,EntmtGroupCode="L014",EntmtGroupDesc="Hiburan yang disediakan oleh jabatan kerajaan, Badan berkanun, pertubuhan, orang, kelab, persatuan, organisasi, jawatankuasa atau institut bagi maksud agama, kebajikan atau khairat"},
            };
            entmtGroup.ForEach(s => context.EntmtGroups.Add(s));
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
                new BusinessCode {CodeNumber="A001",SectorID=1,CodeDesc="Pejabat urusan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A002",SectorID=1,CodeDesc="Kemudahan dan perkhidmatan jagaan kesihatan swasta",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A003",SectorID=1,CodeDesc="Bank dan institusi kewangan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A004",SectorID=1,CodeDesc="Kedai buku dan alat tulis",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A005",SectorID=1,CodeDesc="Institusi pendidikan tinggi swasta, sekolah swasta atau institusi pendidikan swasta",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A006",SectorID=1,CodeDesc="Barangan elektrik atau elektronik atau komputer",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A007",SectorID=1,CodeDesc="Perabot",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A008",SectorID=1,CodeDesc="Hiasan dalaman/barangan hiasan dalaman",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A009",SectorID=1,CodeDesc="Peralatan dan perkakasan rumah/pejabat",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A010",SectorID=1,CodeDesc="Peralatan dan kelengkapan dapur",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A011",SectorID=1,CodeDesc="Perkhidmatan menjahit",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A012",SectorID=1,CodeDesc="Pakaian, tekstil dan alat jahitan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A013",SectorID=1,CodeDesc="Kosmetik dan kelengkapan dandanan diri",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A014",SectorID=1,CodeDesc="Peralatan makmal, saintifik dan perubatan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A015",SectorID=1,CodeDesc="Ubat, farmasi dan produk kesihatan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A016",SectorID=1,CodeDesc="Barang kemas, hiasan diri dan persendirian",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A017",SectorID=1,CodeDesc="Jam",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A018",SectorID=1,CodeDesc="Bagasi",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A019",SectorID=1,CodeDesc="Cenderamata",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A020",SectorID=1,CodeDesc="Barangan optik",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A021",SectorID=1,CodeDesc="Peralatan, aksesori dan perkhidmatan fotografi",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A022",SectorID=1,CodeDesc="Bunga dan tumbuhan tiruan/hidup",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A023",SectorID=1,CodeDesc="Perkhidmatan andaman dan pakaian pengantin",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A024",SectorID=1,CodeDesc="Telefon dan aksesori",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A025",SectorID=1,CodeDesc="Barangan antik",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A026",SectorID=1,CodeDesc="Menjilid buku atau membuat fotokopi",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A027",SectorID=1,CodeDesc="Tukang kunci",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A028",SectorID=1,CodeDesc="Pusat/studio rakaman audio",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A029",SectorID=1,CodeDesc="Stesen minyak/gas/elektrik",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A030",SectorID=1,CodeDesc="Media digital/elektronik dan aksesori berkaitan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A031",SectorID=1,CodeDesc="Barangan logam (untuk sektor pembinaan dan pembuatan)",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A032",SectorID=1,CodeDesc="Alat-alat muzik atau kelengkapan muzik",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A033",SectorID=1,CodeDesc="Kelas kesenian/kebudayaan/kemahiran",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A034",SectorID=1,CodeDesc="Peralatan, bahan dan hiasan landskap",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A035",SectorID=1,CodeDesc="Kenderaan berat",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A036",SectorID=1,CodeDesc="Kereta, motosikal, bot dan jet ski",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A037",SectorID=1,CodeDesc="Mesin dan jentera",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A038",SectorID=1,CodeDesc="Basikal dan aksesori",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A039",SectorID=1,CodeDesc="Peralatan kesihatan, kecergasan dan sukan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A040",SectorID=1,CodeDesc="Produk berasaskan tembakau (seperti rokok/curut dan produk seumpamanya)",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A041",SectorID=1,CodeDesc="Kemasan bangunan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A042",SectorID=1,CodeDesc="Alat permainan dan barangan hobi",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A043",SectorID=1,CodeDesc="Percetakan digital",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A044",SectorID=1,CodeDesc="Kedai barangan runcit/kedai serbaneka (seperti pasar raya dan gedung)",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A045",SectorID=1,CodeDesc="Sewaan kereta",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A046",SectorID=1,CodeDesc="Haiwan peliharaan, dandanan binatang, peralatan dan makanan haiwan dan/atau rumah tumpangan haiwan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A047",SectorID=1,CodeDesc="Agensi teman sosial",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A048",SectorID=1,CodeDesc="Kedai bebas cukai",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A049",SectorID=1,CodeDesc="Pengurusan mayat dan pengebumian",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A050",SectorID=1,CodeDesc="Baja, racun atau kimia-kimia lain yang serupa",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A051",SectorID=1,CodeDesc="Bahan berbahaya yang mudah terbakar tidak termasuk petroleum dan gas",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A052",SectorID=1,CodeDesc="Dobi",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A053",SectorID=1,CodeDesc="Stor kayu",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A054",SectorID=1,CodeDesc="Agensi pelancongan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A055",SectorID=1,CodeDesc="Agensi pekerjaan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A056",SectorID=1,CodeDesc="Jualan tiket pengangkutan awam",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A057",SectorID=1,CodeDesc="Pusat kecantikan dan penjagaan kesihatan",DefaultRate=25f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A058",SectorID=1,CodeDesc="Joki kereta",DefaultRate=0.0f,BaseFee=100.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="A059",SectorID=1,CodeDesc="Pemberi Pinjam Wang",DefaultRate=0.0f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=2},
                new BusinessCode {CodeNumber="B001",SectorID=2,CodeDesc="Gudang/stor makanan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="B002",SectorID=2,CodeDesc="Gudang/stor barang-barang lain",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="B003",SectorID=2,CodeDesc="Gudang/stor bahan merbahaya",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C001",SectorID=3,CodeDesc="Barang-barang berasaskan logam",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C002",SectorID=3,CodeDesc="Media digital dan elektronik",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C003",SectorID=3,CodeDesc="Makanan dan perkakasan haiwan",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C004",SectorID=3,CodeDesc="Baja",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C005",SectorID=3,CodeDesc="Kenderaan, jentera dan mesin",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C006",SectorID=3,CodeDesc="Alat-alat ganti dan aksesori kenderaan bermotor",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C007",SectorID=3,CodeDesc="Basikal",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C008",SectorID=3,CodeDesc="Tayar dan tiub",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C009",SectorID=3,CodeDesc="Bahan binaan",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C010",SectorID=3,CodeDesc="Bateri",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C011",SectorID=3,CodeDesc="Kabel dan wayar",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C012",SectorID=3,CodeDesc="Permaidani dan hamparan",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C013",SectorID=3,CodeDesc="Keranda dan batu nisan",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C014",SectorID=3,CodeDesc="Kosmetik dan kelengkapan dandanan diri",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C015",SectorID=3,CodeDesc="Bahan pencuci, alat-alat mencuci bahan-bahan lain yang seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C016",SectorID=3,CodeDesc="Bahan pengilap",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C017",SectorID=3,CodeDesc="Mengisi gas ke dalam botol atau silinder",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C018",SectorID=3,CodeDesc="Dadah/ubat-ubatan dan keluaran-keluaran farmasi",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C019",SectorID=3,CodeDesc="Fabrik/kulit atau pakaian",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C020",SectorID=3,CodeDesc="Menjahit dan menyulam",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C021",SectorID=3,CodeDesc="Barang-barang elektrik dan elektronik",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C022",SectorID=2,CodeDesc="Barang-barang komputer",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C023",SectorID=2,CodeDesc="Membuat barang-barang perubatan, saintifik dan makmal",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C024",SectorID=2,CodeDesc="Kaca dan cermin",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C025",SectorID=2,CodeDesc="Anggota badan palsu",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C026",SectorID=2,CodeDesc="Produk dibuat daripada gelas serabut, gentian sintetik, kapas tali dan produk seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C027",SectorID=2,CodeDesc="Mercun dan bahan letupan",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C028",SectorID=2,CodeDesc="Gas mudah terbakar",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C029",SectorID=3,CodeDesc="Keluaran petroleum termasuk minyak pelincir dan lain-lain minyak yang seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C030",SectorID=3,CodeDesc="Produk berasaskan emas, perak, tembaga dan bahan-bahan yang seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C031",SectorID=3,CodeDesc="Peralatan sembahyang dan barang-barang lain yang berkaitan",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C032",SectorID=3,CodeDesc="Batu kapur",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C033",SectorID=3,CodeDesc="Cat",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C034",SectorID=3,CodeDesc="Kertas dan hasil-hasil kertas",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C035",SectorID=3,CodeDesc="Plastik, barang-barang daripada bahan plastik atau bahan lain yang seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C036",SectorID=3,CodeDesc="Kaca, barang-barang daripada bahan kaca atau bahan lain yang seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C037",SectorID=3,CodeDesc="Logam, barang-barang daripada bahan logam atau bahan lain yang seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C038",SectorID=3,CodeDesc="Papan dan kayu",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C039",SectorID=3,CodeDesc="Perabot",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C040",SectorID=3,CodeDesc="Barang-barang tembikar",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C041",SectorID=3,CodeDesc="Percetakan (berskala besar)",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C042",SectorID=3,CodeDesc="Pam dan penapis air",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C043",SectorID=3,CodeDesc="Keluaran getah",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C044",SectorID=3,CodeDesc="Tirai dan bidai",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C045",SectorID=3,CodeDesc="Alat tulis/buku/majalah",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C046",SectorID=3,CodeDesc="Barang-barang mainan",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C047",SectorID=3,CodeDesc="Barangan pertanian dan kimia industri",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C048",SectorID=3,CodeDesc="Blok ais",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C049",SectorID=3,CodeDesc="Visual iklan",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C050",SectorID=3,CodeDesc="Makanan dan seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="C051",SectorID=3,CodeDesc="Minuman dan seumpamanya",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="D001",SectorID=4,CodeDesc="Alat ganti dan aksesori",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="D002",SectorID=4,CodeDesc="Pemasangan penyaman udara di kenderaan",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="D003",SectorID=4,CodeDesc="Kenderaan bermotor dan kenderaan marin",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="D004",SectorID=4,CodeDesc="Tayar",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="D005",SectorID=4,CodeDesc="Mencuci dan/atau mengilap kereta",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="D006",SectorID=4,CodeDesc="Menyembur cat, selulosa dan bahan-bahan kimia lain",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="D007",SectorID=4,CodeDesc="Kerja-kerja kejuruteraan",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="D008",SectorID=4,CodeDesc="Kerja-kerja kimpalan",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="D009",SectorID=4,CodeDesc="Pertukangan batu, kayu, kaca dan logam (termasuk papan iklan) ",DefaultRate=2.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="E001",SectorID=5,CodeDesc="Menternak burung walit",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="E002",SectorID=5,CodeDesc="Menternak lebah, lintah, cacing dan seumpamanya",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="E003",SectorID=5,CodeDesc="Tempat pembiakan haiwan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="E004",SectorID=5,CodeDesc="Rumah sembelihan binatang",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="E005",SectorID=5,CodeDesc="Kolam pancing",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="E006",SectorID=5,CodeDesc="Semaian tumbuhan",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="F001",SectorID=6,CodeDesc="Mana-mana aktiviti perniagaan yang tidak termasuk dalam Jadual",DefaultRate=1.5f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="G001",SectorID=7,CodeDesc="Restoran/kedai makan/gerai makan/kios makanan",DefaultRate=10.0f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="G002",SectorID=7,CodeDesc="Menjual makanan/minuman (tanpa tempat makan)",DefaultRate=10.0f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="G003",SectorID=7,CodeDesc="Katering makanan",DefaultRate=10.0f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="G004",SectorID=7,CodeDesc="Kantin sekolah",DefaultRate=10.0f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="G005",SectorID=7,CodeDesc="Kantin pejabat",DefaultRate=10.0f,BaseFee=0.0f,Period=1,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="H001",SectorID=8,CodeDesc="Hotel (Kelas Pertama)",DefaultRate=0.0f,BaseFee=150.0f,Period=2,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="H002",SectorID=8,CodeDesc="Hotel (Kelas Kedua)",DefaultRate=0.0f,BaseFee=100.0f,Period=2,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="H003",SectorID=8,CodeDesc="Hotel (Kelas Ketiga)",DefaultRate=0.0f,BaseFee=30.0f,Period=2,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="H004",SectorID=8,CodeDesc="Lodging House/Rumah Tumpangan",DefaultRate=0.0f,BaseFee=30.0f,Period=2,Mode=1,PeriodQuantity=1},
                new BusinessCode {CodeNumber="I001",SectorID=9,CodeDesc="Pengurusan Skrap/Dealing in Scrap",DefaultRate=0.0f,BaseFee=25.0f,Period=2,Mode=1,PeriodQuantity=3},
            };
            businesscode.ForEach(s => context.BusinessCodes.Add(s));
            context.SaveChanges();

            var departments = new List<Department>
            {
                new Department {DepartmentCode="Pelesenan",DepartmentDesc="Bahagian Pelesenan",Internal=1},
                new Department {DepartmentCode="ICT",DepartmentDesc="Jabatan Pengurusan Maklumat",Internal=1},
                new Department {DepartmentCode="BPP",DepartmentDesc="Jabatan Perancangan & Kawalan Bangunan",Internal=1},
                new Department {DepartmentCode="JPPPH",DepartmentDesc="Jabatan Penilaian, Pelaburan dan Pengurusan Harta",Internal=1},
                new Department {DepartmentCode="UKS",DepartmentDesc="Unit Kesihatan",Internal=1},
                new Department {DepartmentCode="PKPE",DepartmentDesc="Pejabat Ketua Pegawai Eksekutif",Internal=1},
                new Department {DepartmentCode="JBPM",DepartmentDesc="Jabatan Bomba & Penyelamat Malaysia",Internal=2},
                new Department {DepartmentCode="PDRM",DepartmentDesc="Polis Diraja Malaysia",Internal=2},
                new Department {DepartmentCode="JKDM",DepartmentDesc="Jabatan Kastam Diraja Malaysia",Internal=2},
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
                new Individual{FullName="Ali Bin Abu",MykadNo="710213-12-4820",NationalityID=1,PhoneNo="0108103140",AddressIC="No.3, Kg. Tg. Aru, Jalan Tg. Aru, 87000 W.P.Labuan",IndividualEmail="aliabu@yahoo.com",Gender=1},
                new Individual{FullName="Siti Aminah",MykadNo="610122-12-4933",NationalityID=1,PhoneNo="0112546778",AddressIC="Lot 20, Blok F, Taman Mutiara, 87000 W.P.Labuan",IndividualEmail="sitiaminah@gmail.com",Gender=2},
                new Individual{FullName="Chin Chee Kiong",MykadNo="500101-12-5129",NationalityID=1,PhoneNo="0148552370",AddressIC="Lot 13, Blok D, Jalan Merdeka, Pusat Bandar, 87000 W.P.Labuan",IndividualEmail="chinchee70@gmail.com",Gender=1},
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

            var AppStatus = new List<AppStatus>
            {
                new AppStatus {StatusDesc="Draft created" ,PercentProgress =1},
                new AppStatus {StatusDesc="Submitted to clerk" ,PercentProgress =5},
                new AppStatus {StatusDesc="Pending client document submission" ,PercentProgress =10},
                new AppStatus {StatusDesc="Processing by Clerk" ,PercentProgress =20},
                new AppStatus {StatusDesc="Route Unit" ,PercentProgress =30},
                new AppStatus {StatusDesc="Meeting" ,PercentProgress =40},
                new AppStatus {StatusDesc="Awaiting Director Response" ,PercentProgress =50},
                new AppStatus {StatusDesc="Awaiting CEO Approval" ,PercentProgress =60},
                new AppStatus {StatusDesc="Processing Letter by Clerk" ,PercentProgress =70},
                new AppStatus {StatusDesc="Letter of notification (Approved)" ,PercentProgress =80},
                new AppStatus {StatusDesc="Letter of notification (Rejected)" ,PercentProgress =80},
                new AppStatus {StatusDesc="Letter of notification (Approved with Terms & Conditions)" ,PercentProgress =80},
                new AppStatus {StatusDesc="Pending payment" ,PercentProgress =90},
                new AppStatus {StatusDesc="License Generated" ,PercentProgress =100},
            };
            AppStatus.ForEach(s => context.AppStatus.Add(s));
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
                new Users {FullName="Abd Aziz Bin Hamzah",Username="aziz",Password="rGWQ/rZGq74=",Email="aziz.h@pl.gov.my", RoleTemplateID=6,DepartmentID=1},
                new Users {FullName="Soffiyan Bin Hadis",Username="soffiyan",Password="rGWQ/rZGq74=",Email="soffiyan.hadis@pl.gov.my", RoleTemplateID=4,DepartmentID=1},
                new Users {FullName="Hjh. Simai Binti Md Jamil",Username="simai",Password="rGWQ/rZGq74=",Email="simai@pl.gov.my", RoleTemplateID=4,DepartmentID=1},
                new Users {FullName="Suwardi Binti Muali",Username="suwardi",Password="rGWQ/rZGq74=",Email="suwardi.muali.pl@1govuc.gov.my", RoleTemplateID=2,DepartmentID=1},
                new Users {FullName="Adey Suhaimi Bin Suhaili",Username="adey",Password="rGWQ/rZGq74=",Email="adey.suhaimi.pl@1govuc.gov.my", RoleTemplateID=4,DepartmentID=1},
                new Users {FullName="Azean Irdawati Binti Wahid",Username="azean",Password="rGWQ/rZGq74=",Email="azean.wahid.pl@1govuc.gov.my", RoleTemplateID=4,DepartmentID=1},
                new Users {FullName="Kazlina Binti Kassim",Username="kazlina",Password="rGWQ/rZGq74=",Email="kazlina@yahoo.com", RoleTemplateID=4,DepartmentID=1},
                new Users {FullName="Johaniza Binti Jonait",Username="johaniza",Password="rGWQ/rZGq74=",Email="johaniza@yahoo.com", RoleTemplateID=4,DepartmentID=1},
                new Users {FullName="Rafidah Binti Mohd Isa",Username="rafidah",Password="rGWQ/rZGq74=",Email="rafidah@yahoo.com", RoleTemplateID=2,DepartmentID=1},
                new Users {FullName="Ahmad Jais Bin Halon",Username="ahmadjais",Password="rGWQ/rZGq74=",Email="ahmad.jais@yahoo.com", RoleTemplateID=2,DepartmentID=1},
                new Users {FullName="YBHG. Datuk Azhar Bin Ahmad",Username="kpe",Password="rGWQ/rZGq74=",Email="azharahmad@pl.gov.my", RoleTemplateID=7,DepartmentID=1},
                new Users {FullName="Mazalan Bin Hassin",Username="mazalan",Password="rGWQ/rZGq74=",Email="mazalan.hassin@pl.gov.my", RoleTemplateID=8,DepartmentID=1},
                new Users {FullName="R. Norasliana Binti Ramlee",Username="norasliana",Password="rGWQ/rZGq74=",Email="ana.ramli@pl.gov.my", RoleTemplateID=8,DepartmentID=1},
                new Users {FullName="Jabatan Bomba & Penyelamat Malaysia",Username="bomba",Password="rGWQ/rZGq74=",Email="jbpm_labuan.bomba@1govuc.gov.my", RoleTemplateID=5,DepartmentID=1},
                new Users {FullName="Ronny Jimmy",Username="ronny",Password="rGWQ/rZGq74=",Email="ronnyrtg@yahoo.com", RoleTemplateID=8,DepartmentID=1},
            };
            users.ForEach(s => context.Users.Add(s));
            context.SaveChanges();

            var requireddocs = new List<RequiredDoc>
            {
                new RequiredDoc {RequiredDocDesc="Borang Komposit bagi Permohonan Lesen Premis Perniagaan dan Iklan"},
                new RequiredDoc {RequiredDocDesc="Satu (1) Salinan Kad Pengenalan ATAU Pasport"},
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
            };
            requireddocs.ForEach(s => context.RequiredDocs.Add(s));
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

            var hawkerCodes = new List<HawkerCode>
            {
                new HawkerCode {HCodeNumber="J001",HawkerCodeDesc="Penjaja Bergerak",Fee=48.0f,Period=1,PeriodQuantity=1,Mode=4},
                new HawkerCode {HCodeNumber="J002",HawkerCodeDesc="Penjaja Statik: Makanan",Fee=10.0f,Period=2,PeriodQuantity=1,Mode=4},
                new HawkerCode {HCodeNumber="J003",HawkerCodeDesc="Penjaja Statik: Selain daripada makanan",Fee=15.0f,Period=2,PeriodQuantity=1,Mode=4},
                new HawkerCode {HCodeNumber="J004",HawkerCodeDesc="Penjaja Sementara Bulanan",Fee=15.0f,Period=2,PeriodQuantity=1,Mode=4},
                new HawkerCode {HCodeNumber="J004",HawkerCodeDesc="Penjaja Sementara Harian",Fee=0.5f,Period=4,PeriodQuantity=1,Mode=4},
                new HawkerCode {HCodeNumber="J005",HawkerCodeDesc="Pasar Malam",Fee=1.0f,Period=4,PeriodQuantity=1,Mode=4},
            };
            hawkerCodes.ForEach(s => context.HawkerCodes.Add(s));
            context.SaveChanges();

            var stallCodes = new List<StallCode>
            {
                new StallCode {SCodeNumber="K001",StallCodeDesc="Ikan",Fee=30.0f,Period=2,PeriodQuantity=1,Mode=4},
                new StallCode {SCodeNumber="K002",StallCodeDesc="Ayam",Fee=30.0f,Period=2,PeriodQuantity=1,Mode=4},
                new StallCode {SCodeNumber="K003",StallCodeDesc="Daging",Fee=30.0f,Period=2,PeriodQuantity=1,Mode=4},
                new StallCode {SCodeNumber="K004",StallCodeDesc="Sayur-sayuran/buah-buahan/telur",Fee=20.0f,Period=2,PeriodQuantity=1,Mode=4},
                new StallCode {SCodeNumber="K005",StallCodeDesc="Barang-barang lain",Fee=20.0f,Period=2,PeriodQuantity=1,Mode=4},
                new StallCode {SCodeNumber="K006",StallCodeDesc="Pasar Persendirian - Luas lantai 70 meter persegi atau lebih",Fee=225.0f,Period=1,PeriodQuantity=1,Mode=4},
                new StallCode {SCodeNumber="K007",StallCodeDesc="Pasar Persendirian - Luas lantai kurang daripada 70 meter persegi",Fee=175.0f,Period=1,PeriodQuantity=1,Mode=4},
            };
            stallCodes.ForEach(s => context.StallCodes.Add(s));
            context.SaveChanges();

            var bannerCodes = new List<BannerCode>
            {
                new BannerCode {BCodeNumber="O001",BannerCodeDesc="Iklan Tidak Bercahaya",ProcessingFee=25.0f,ExtraFee=25.0f,Period=1,PeriodQuantity=1,PeriodFee=50.0f,Mode=2},
                new BannerCode {BCodeNumber="O002",BannerCodeDesc="Iklan Bercahaya",ProcessingFee=25.0f,ExtraFee=25.0f,Period=1,PeriodQuantity=1,PeriodFee=100.0f,Mode=2},
                new BannerCode {BCodeNumber="O003",BannerCodeDesc="Iklan Kecil",ProcessingFee=25.0f,ExtraFee=25.0f,Period=1,PeriodQuantity=1,PeriodFee=50.0f,Mode=2},
                new BannerCode {BCodeNumber="O004",BannerCodeDesc="Iklan yang mengunjur lebih daripada 15 sentimeter melebihi bangunan – tidak bercahaya",ProcessingFee=25.0f,ExtraFee=25.0f,Period=1,PeriodQuantity=1,PeriodFee=100.0f,Mode=2},
                new BannerCode {BCodeNumber="O005",BannerCodeDesc="Iklan yang mengunjur lebih daripada 15 sentimeter melebihi bangunan – bercahaya",ProcessingFee=25.0f,ExtraFee=25.0f,Period=1,PeriodQuantity=1,PeriodFee=200.0f,Mode=2},
                new BannerCode {BCodeNumber="O006",BannerCodeDesc="Tanda Langit",ProcessingFee=25.0f,Period=1,PeriodQuantity=1,PeriodFee=100.0f,Mode=2},
            };
            bannerCodes.ForEach(s => context.BannerCodes.Add(s));
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

            var liquorCodes = new List<LiquorCode>
            {
                new LiquorCode {LCodeNumber="N001",LiquorCodeDesc="Runcit",DefaultHours="7.00pg. hingga 9.00mlm",ExtraHourFee=0.0f,Period=2,PeriodQuantity=1,PeriodFee=35.0f,Mode=3},
                new LiquorCode {LCodeNumber="N002",LiquorCodeDesc="Borong",DefaultHours="7.00pg. hingga 9.00mlm",ExtraHourFee=0.0f,Period=2,PeriodQuantity=1,PeriodFee=45.0f,Mode=3},
                new LiquorCode {LCodeNumber="N003",LiquorCodeDesc="Rumah Awam (Kelas 1)",DefaultHours="10.00pg. hingga 12.00mlm",ExtraHourFee=30.0f,Period=2,PeriodQuantity=1,PeriodFee=110.0f,Mode=3},
                new LiquorCode {LCodeNumber="N004",LiquorCodeDesc="Rumah Awam (Kelas 2)",DefaultHours="10.00pg. hingga 10.00mlm",ExtraHourFee=30.0f,Period=2,PeriodQuantity=1,PeriodFee=80.0f,Mode=3},
                new LiquorCode {LCodeNumber="N005",LiquorCodeDesc="Rumah Awam (Kelas 3)",DefaultHours="10.00pg. hingga 9.00mlm",ExtraHourFee=30.0f,Period=2,PeriodQuantity=1,PeriodFee=55.0f,Mode=3},
                new LiquorCode {LCodeNumber="N006",LiquorCodeDesc="Rumah Bir (Kelas 1)",DefaultHours="10.00pg. hingga 12.00mlm",ExtraHourFee=15.0f,Period=2,PeriodQuantity=1,PeriodFee=55.0f,Mode=3},
                new LiquorCode {LCodeNumber="N007",LiquorCodeDesc="Rumah Bir (Kelas 2)",DefaultHours="10.00pg. hingga 10.00mlm",ExtraHourFee=15.0f,Period=2,PeriodQuantity=1,PeriodFee=35.0f,Mode=3},
                new LiquorCode {LCodeNumber="N008",LiquorCodeDesc="Lesen Sementara",DefaultHours="Tidak Berkenaan",ExtraHourFee=0.0f,Period=4,PeriodQuantity=1,PeriodFee=30.0f,Mode=3},
                new LiquorCode {LCodeNumber="N009",LiquorCodeDesc="Lesen Kedai Todi (Estet/Swasta)",DefaultHours="Tidak Berkenaan",ExtraHourFee=0.0f,Period=2,PeriodQuantity=1,PeriodFee=4.5f,Mode=3},
                new LiquorCode {LCodeNumber="N010",LiquorCodeDesc="Lesen Sadapan Todi",DefaultHours="Tidak Berkenaan",ExtraHourFee=0.0f,Period=2,PeriodQuantity=1,PeriodFee=15.0f,Mode=3},
            };
            liquorCodes.ForEach(s => context.LiquorCodes.Add(s));
            context.SaveChanges();

            var entmtObjects = new List<EntmtObject>
            {
                new EntmtObject {EntmtObjectDesc="Billiard/Snuker",ObjectFee=10.0f,ObjectName="meja",Period=2,PeriodQuantity=1},
                new EntmtObject {EntmtObjectDesc="Boling",ObjectFee=5.0f,ObjectName="lorong",Period=2,PeriodQuantity=1},
                new EntmtObject {EntmtObjectDesc="Pameran Filem Sinematograf - Dalam penggung/panggung wayang",ObjectFee=5.0f,ObjectName="pertunjukan",Period=0,PeriodQuantity=0},
                new EntmtObject {EntmtObjectDesc="Pameran Filem Sinematograf - Di tempat terbuka",ObjectFee=5.0f,Period=4,PeriodQuantity=1},
                new EntmtObject {EntmtObjectDesc="Sarkas",BaseFee=5.0f,ObjectFee=2.5f,ObjectName="pertunjukan",Period=0,PeriodQuantity=0},
                new EntmtObject {EntmtObjectDesc="Hiburan dengan Mesin Hiburan - Kiddy Rides",ObjectFee=2.0f,ObjectName="mesin",Period=2,PeriodQuantity=1},
                new EntmtObject {EntmtObjectDesc="Hiburan dengan Mesin Hiburan - Mesin video",ObjectFee=5.0f,ObjectName="mesin",Period=2,PeriodQuantity=1},
                new EntmtObject {EntmtObjectDesc="Hiburan di Pub/Coffee House/Lounge/Disko/Dewan Tarian - Muzik dan nyanyian",ObjectFee=4.0f,ObjectName="sehingga 12 malam",Period=4,PeriodQuantity=1},
                new EntmtObject {EntmtObjectDesc="Hiburan di Pub/Coffee House/Lounge/Disko/Dewan Tarian - Muzik dan nyanyian",ObjectFee=8.0f,ObjectName="selepas 12 malam",Period=4,PeriodQuantity=1},
                new EntmtObject {EntmtObjectDesc="Hiburan di Pub/Coffee House/Lounge/Disko/Dewan Tarian - Tarian",ObjectFee=4.0f,ObjectName="sehingga 12 malam",Period=4,PeriodQuantity=1},
                new EntmtObject {EntmtObjectDesc="Hiburan di Pub/Coffee House/Lounge/Disko/Dewan Tarian - Tarian",ObjectFee=14.0f,ObjectName="selepas 12 malam",Period=4,PeriodQuantity=1},
                new EntmtObject {EntmtObjectDesc="Pameran",ObjectFee=5.0f,ObjectName="gerai",Period=4,PeriodQuantity=1},
                new EntmtObject {EntmtObjectDesc="Pertunjukan Fesyen oleh Artis Profesional/Pertandingan Ratu Cantik",ObjectFee=10.0f,ObjectName="artis/peserta",Period=4,PeriodQuantity=1},
                new EntmtObject {EntmtObjectDesc="Pesta ria",ObjectFee=2.0f,ObjectName="gerai",Period=4,PeriodQuantity=1},
                new EntmtObject {EntmtObjectDesc="Hiburan Juke Box",ObjectFee=10.0f,ObjectName="mesin",Period=2,PeriodQuantity=1},
                new EntmtObject {EntmtObjectDesc="Pertunjukan Patung",ObjectFee=5.0f,Period=4,PeriodQuantity=1},
                new EntmtObject {EntmtObjectDesc="Apa-apa hiburan di luar kuil",ObjectFee=5.0f,Period=4,PeriodQuantity=1},
                new EntmtObject {EntmtObjectDesc="Apa-apa hiburan lain",ObjectFee=10.0f,Period=4,PeriodQuantity=1},
            };
            entmtObjects.ForEach(s => context.EntmtObjects.Add(s));
            context.SaveChanges();

            var entmtCodes = new List<EntmtCode>
            {
                new EntmtCode {EntmtGroupID=1,EntmtCodeDesc="Tidak melebihi 200 kerusi",Fee=400.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=1,EntmtCodeDesc="200 hingga 400 kerusi",Fee=800.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=1,EntmtCodeDesc="400 hingga 600 kerusi",Fee=1000.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=1,EntmtCodeDesc="600 hingga 800 kerusi",Fee=1400.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=1,EntmtCodeDesc="800 hingga 1,000 kerusi",Fee=1600.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=1,EntmtCodeDesc="1,000 hingga 1,200 kerusi",Fee=1800.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=1,EntmtCodeDesc="Lebih 1,200 kerusi",Fee=2000.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=1,EntmtCodeDesc="Lesen Sementara",Fee=10.0f,Period=4,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=2,EntmtCodeDesc="Tidak melebihi 200 kerusi",Fee=400.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=2,EntmtCodeDesc="Melebihi 200 kerusi tetapi tidak melebihi 400 kerusi",Fee=900.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=2,EntmtCodeDesc="Melebihi 400 kerusi tetapi tidak melebihi 600 kerusi",Fee=1100.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=2,EntmtCodeDesc="Melebihi 600 kerusi tetapi tidak melebihi 800 kerusi",Fee=1550.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=2,EntmtCodeDesc="Melebihi 800 kerusi tetapi tidak melebihi 1,000 kerusi",Fee=1800.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=2,EntmtCodeDesc="Melebihi 1,000 kerusi tetapi tidak melebihi 1,200 kerusi",Fee=2000.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=2,EntmtCodeDesc="Lebih 1,200 kerusi",Fee=2200.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=2,EntmtCodeDesc="Lesen Sementara",Fee=20.0f,Period=4,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=3,EntmtCodeDesc="Luas lantai tidak melebihi 30 meter persegi",Fee=300.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=3,EntmtCodeDesc="Luas lantai melebihi 30 meter persegi tetapi tidak melebihi 60 meter persegi",Fee=500.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=3,EntmtCodeDesc="Luas lantai melebihi 60 meter persegi tetapi tidak melebihi 90 meter persegi",Fee=800.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=3,EntmtCodeDesc="Lebih 90 meter persegi",Fee=1100.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=4,EntmtCodeDesc="Pusat Hiburan/Taman Hiburan (Luar Bangunan)",Fee=10.0f,Period=4,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=5,EntmtCodeDesc="Lorong Boling",Fee=1100.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=6,EntmtCodeDesc="Gelanggang Luncur",Fee=1000.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=7,EntmtCodeDesc="Mesin Hiburan (Kiddy Rides/Juke Box) di tempat selain daripada Pusat Hiburan",ObjectFee=20.0f,ObjectName="mesin",Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=8,EntmtCodeDesc="Luas lantai tidak melebihi 30 meter persegi",Fee=400.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=8,EntmtCodeDesc="30 hingga 60 meter persegi",Fee=600.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=8,EntmtCodeDesc="60 hingga 90 meter persegi",Fee=900.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=8,EntmtCodeDesc="Lebih 90 meter persegi",Fee=1200.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=8,EntmtCodeDesc="Kabaret - Lebih 90 meter persegi",Fee=2200.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=9,EntmtCodeDesc="Tidak melebihi 5 meja",Fee=500.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=9,EntmtCodeDesc="Melebihi 5 meja tetapi tidak melebihi 10 meja",Fee=1000.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=9,EntmtCodeDesc="Melebihi 10 meja tetapi tidak melebihi 20 meja",Fee=1500.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=9,EntmtCodeDesc="Melebihi 20 meja tetapi tidak melebihi 30 meja",Fee=2000.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=9,EntmtCodeDesc="Melebihi 30 meja tetapi tidak melebihi 40 meja",Fee=2500.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=9,EntmtCodeDesc="Melebihi 40 meja tetapi tidak melebihi 50 meja",Fee=3000.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=9,EntmtCodeDesc="Melebihi 50 meja",BaseFee=3000.0f,ObjectFee=50.0f,ObjectName="meja",Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=10,EntmtCodeDesc="Tidak melebihi 1000 kerusi",Fee=400.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=10,EntmtCodeDesc="Melebihi 1000 kerusi",Fee=800.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=10,EntmtCodeDesc="Lesen sementara",Fee=50.0f,Period=4,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=11,EntmtCodeDesc="Luas lantai tidak melebihi 100 meter persegi",Fee=100.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=11,EntmtCodeDesc="Luas lantai melebihi 100 meter persegi tetapi tidak melebihi 150 meter persegi",Fee=200.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=11,EntmtCodeDesc="Luas lantai melebihi 150 meter persegi tetapi tidak melebihi 200 meter persegi",Fee=300.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=11,EntmtCodeDesc="Luas lantai melebihi 200 meter persegi",Fee=500.0f,Period=1,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=11,EntmtCodeDesc="Lesen sementara",Fee=10.0f,Period=4,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=12,EntmtCodeDesc="Apa-apa hiburan bagi maksud pendidikan yang disediakan oleh mana-mana sekolah, university, maktab, PIBG, kumpulan guru atau murid",Fee=0.0f,Period=2,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=13,EntmtCodeDesc="Apa-apa sukan atau permainan yang bertaraf amatur yang disediakan oleh sesuatu pertubuhan atau badan",Fee=0.0f,Period=2,PeriodQuantity=1},
                new EntmtCode {EntmtGroupID=14,EntmtCodeDesc="Apa-apa hiburan yang disediakan oleh mana-mana jabatan ke bagi maksud keagamaan, kebajikan atau khairat, tetapi tidak termasuk hiburan di luar bangunan kuil.",Fee=0.0f,Period=2,PeriodQuantity=1},

            };
            entmtCodes.ForEach(s => context.EntmtCodes.Add(s));
            context.SaveChanges();
        }
    }
}
