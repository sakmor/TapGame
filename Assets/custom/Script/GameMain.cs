using AOT;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameMain : MonoBehaviour
{
	[SerializeField] private Button m_StartBtn;

	public Button LogoUIBtn;
	public GameObject StartUI;
	public GameObject GameUI;
	public GameObject GameEndUI;
	public AudioSource GameMusic;
	public VideoPlayer StartVideo;
	public Text TimeText;

	[Header("Input Name Panel")]
	public Button RandNameBtn;
	public GameObject InputNamePanel;
	public InputField InputNameField;
	public Button NameStartGameBtn;
	public string UserName;

	[Header("Monster")]
	public RectTransform MonsterTrans;
	public UIAnim MonsterU;
	public Image MonsterHPBar;
	public Text MonsterNameText;
	public List<string> MonsterNames;
	[Header("Monster Image")]
	public List<Sprite> Level1_Monster;
	public List<Sprite> Level2_Monster;
	public List<Sprite> Level3_Monster;
	[Header("Monster HP")]
	public int L1_HP = 10;
	public int L2_HP = 20;
	public int L3_HP = 25;

	public int NowMonsterHP;
	public int mMonsterMaxHP;
	public int Score = 0;
	private int mNowLevel = 0;
	private float mBounsTimeScore = 300;
	public bool GameStart;

	[Header("HP Anim")]
	public List<Transform> HPList;
	public Transform HPPrefab;
	public Transform HPRoot;

	[Header("GameOver")]
	public List<RankNameLabel> Ranks;
	public GameObject GameOverPage;
	public Text ScoreText;
	public Text GameScoreText;
	public List<string> MarqueeStrs;
	public GameObject MarqueeRoot;
	public Text WinTitleMarquee;
	public Button QuitBtn;
	public Button ReStartBtn;
	Tweener mPosTweener, mColorTweener;

	//[Header("SQLServer")]
	//public GameDataSql SQLServer;
	System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//�ޥ�stopwatch����

	public AudioClip DamSound;
	public AudioSource ASource;

	void Start()
	{
		HPList = new List<Transform>();
		// 		Application.focusChanged += (b) =>
		// 		{
		// #if !UNITY_EDITOR
		//             if(b)
		//                 MobileOrientationDetector.ScreenLock();
		// #endif
		// 			Debug.Log($"focusChanged : {b}");
		// 		};

		LogoUIBtn.onClick.AddListener(() =>
		{
			LogoUIBtn.gameObject.SetActive(false);
			StartUI.SetActive(true);
			StartVideo.Play();
			// MobileOrientationDetector.ScreenLock();
		});

		m_StartBtn.onClick.AddListener(() =>
		{
			StartUI.SetActive(false);
			StartVideo.Stop();

			InputNamePanel.gameObject.SetActive(false);

			GameUI.SetActive(true);
			GameMusic.Play();

			GameStart = true;
			// #if !UNITY_EDITOR
			//             MobileOrientationDetector.ScreenLock();
			// #endif
			SetLevel();

			//InputNamePanel.gameObject.SetActive(true);

		});

		RandNameBtn.onClick.AddListener(() =>
		{
			//InputNameField.text = GetRandomNickName();
			//NameStartGameBtn.interactable = true;
		});

		InputNameField.onValueChanged.AddListener(s =>
		{
			NameStartGameBtn.interactable = !string.IsNullOrEmpty(InputNameField.text);
			UserName = CheckInputText(InputNameField.text);
		});

		NameStartGameBtn.onClick.AddListener(() =>
		{
			DoGameStart();
		});

		QuitBtn.onClick.AddListener(() =>
		{
			GameEndUI.SetActive(true);
			Invoke("CloseGame", 5);
		});

		ReStartBtn.onClick.AddListener(() =>
		{
			Score = 0;
			GameOverPage.SetActive(false);
			GameUI.SetActive(true);
			GameMusic.Play();
			SetLevel();
		});

		// m_StartBtn.gameObject.SetActive(false);
		// StartVideo.prepareCompleted += StartVideo_prepareCompleted;
		// StartVideo.Prepare();
		// #if !UNITY_EDITOR
		//         MobileOrientationDetector.Init();
		// #endif
	}

	public void DoGameStart()
	{
		InputNamePanel.gameObject.SetActive(false);

		GameUI.SetActive(true);
		GameMusic.Play();
		GameStart = true;
		// #if !UNITY_EDITOR
		//             MobileOrientationDetector.ScreenLock();
		// #endif
		SetLevel();
	}

	private void Update()
	{
		if (GameStart)
			mBounsTimeScore = mBounsTimeScore - Time.deltaTime;

		TimeText.text = $" Time	: {sw.Elapsed.Minutes.ToString("#00")}:{sw.Elapsed.Seconds.ToString("#00")}";
		GameScoreText.text = $"Score	: {Score}";
		ScoreText.text = Score.ToString("#00000");
	}

	public void PlaySound(AudioClip sound)
	{
		if (sound != null)
			ASource.PlayOneShot(sound);
	}

	public string GetRandomNickName()
	{
		string[] nickArray = null;
		string names = "Sam,Jack";//"Abigail,�R��\��,Ada,�R�F,Agatha,���[��,Adelaide,�R�o�p�w,Adelina,�R�o�D��,Alethea,��g�F��,Aggie,���N,Agnes,���g��,Aileen,�R�Y,Alex,�R���J��,Alexandra,�Ⱦ��s����,Alexis,�Ⱦ��J�贵,Alice,�R�R��,Alison,�����,Amanda,�R�ҹF,Amy,�R��,Angela,�w�X��,Angie,�w�N,Anita,�w�g��,Anne,�w,Anna,�w�R,Annabel,�w�Ǩ���,Annie,�w�g,Annette,�w�g�S,Anthea,�w����,Antonia,�w�F����,Audrey,���L�R,Allson,�R�R�^,Alma,�R��,Althea,�R�g��,Angelica,�w�N����,Aspasia,��_�L,Aurelian,�Z��g��,Ava,�R��,Avis,���,Beata,�_�F,Belle,�_��,Babs,�ݥ���,Barbara,�ݪݩ�,Beatrice,����S�R��,Becky,�_�X,Belinda,��Y�F,Bernadette,�B���L�S,Beryl,������,Betty,����,Bid,��w,Brenda,���۹F,Bridget,�����_�S,Brittany,�����Z�g,Bertha,�ղ�,Bonny,�i�g,Camilla,�d����,Candice,���}��,Carla,�d��,Carol,�d��,Caroline,�d���Y,Carrie,�d�R,Catherine,���ĵY,Cathy,�ͦ�,Cecilia,������,Cecily,����,Celia,�����,Charlene,�d��,Charlotte,�L���S,Cherry,����,Cheryl,�¨���,Chloe,�J����,Chris,�J����,Chrissie,�J����,Christina,�J�������R,Christine,�J������,Cindy,����,Clare,�J�ܺ�,Claudia,�J�Ҧa��,Cleo,�J�Q��,Connie,�d��,Constance,�d���Z��,Crystal,�J������,Candida,�͸��F,Carmen,�d�p,Celestine,��ӵ���,Charissa,�d���,Colleen,�i�Y,Cora,�i�R,Corinna,�i���,Cynthia,�����,Dulcie,�纸��,Dottie,�h��,Daisy,�L��,Daphne,�F�ҩg,Dawn,��,Deb,�L��,Debby,�L��,Deborah,�L�թ�,Deirdre,�}���w�R,Delia,�}����,Della,�L��,Denise,������,Di,�L,Diana,�L�w�R,Diane,�L�w,Dolly,�h�Q,Donna,���R,Dora,�h��,Doreen,�h�Y,Doris,��ֵ�,Dorothy,���ǵ�,Dot,�h�S,Elva,�㫽,Edith,��}��,Edna,�R�w�R,Eileen,�R�Y,Elaine,��ܮ�,Eleanor,�R����,Eleanora,�R���թ�,Eliza,��ܤ�,Elizabeth,������,Ella,�R��,Ellen,�R��,Ellie,�R��,Elsa,�R��,Elsie,�R����,Elspeth,�R����ث�,Emily,�R�e��,Emma,�R��,Erica,�R���d,Ethel,�R�뺸,Eunice,�שg��,Eva,�쫽,Eve,�쫽,Evelyn,��ܵY,Eugenia,�ת��g��,Eulalia,�װǲ���,Evadne,���g,Evangeline,�̤�N�Y,Faustina,�ִ��ӮR,Fay,��,Felicity,�O���踦,Fidelia,�O�w����,Fiona,�O�ڮR,Flo,����,Flora,������,Florence,��ù�۴�,Felicia,�S�R���,Flavia,�֨ӵ��,Frieda,�k�g�F,Freda,���p�F,Florrie,������,Fran,����,Frances,�����贵,Frankie,���԰�,Gene,��,Georgia,��v��,Georgie,��v,Georgina,��v�R,Geraldine,�N�Ժ��B,Germaine,�N��,Gertie,�渦,Gertrude,��S�|�w,Gill,�N��,Gillian,�N�Q��,Ginny,�^�g,Gladys,��ԭ}��,Glenda,��۹F,Gloria,��������,Grace,���紵,Gracie,��p��,Ashley,�R����,Gwen,���,Gwendoline,��żw�L,Hannah,�~��,Harriet,���R���S,Hazel,�¯���,Heather,����,Helen,����,Henrietta,�먽�J��,Hilary,�߿ղ�,Hilda,�ƺ��F,Hedda,���F,Hedwig,����,Helga,����,Hortensia,���Ц��,Isabella,��﨩��,Ivy,���,Ida,�R�F,Ingrid,�^�樽�w,Irene,�R�Y,Iris,�R����,Isabel,��﨩��,Jemima,�۬���,Juliana,�����w�R,Jan,²,Jane,��,Janet,�ég�w,Janey,�ég,Janice,��g��,Jackie,�N��,Jacqueline,��Q�Y,Jean,�î�,Jeanie,�ég,Jennifer,�ég��,Jenny,�ég,Jess,�䴵,Jessica,���d,Jessie,���,Jill,�N��,Jo,��,Joan,ã,Joanna,��w�R,Joanne,��w�g,Jocelyn,�촵�Y,Josephine,�����,Josie,���,Jode,��},Joyce,����,Judith,���}��,Judy,����,Julia,������,Julie,����,Juliet,���R��,June,ã,Karen,�ͭ�,Kathleen,�ͷ�Y,Kate,�ͯS,Kathy,�ͦ�,Katie,�͸�,Kay,��,Kelly,�Ͳ�,Kim,��,Kimberly,���f��,Kirsten,�J�R���Z,Kitty,��,Katharine,���ĵY,Kit,�ͼw,Leila,����,Laura,�ҩ�,Lauretta,���p��,Lesley,�ܴ���,Libby,����,Lilian,����,Lily,����,Linda,�Y�F,Lindsay,�Y��,Lisa,�R��,Livia,������,Liz,�R��,Lois,���쵷,Lori,ù��,Lorna,���R,Louisa,������,Louise,������,Lucia,�S���,Lucinda,�S���F,Lucy,�S��,Lydia,���}��,Lynn,�L��,Leslie,�����R,Lucile,�S�躸,LuLu,�S�S,Mabel,������,Madeleine,���w�Y,Madge,���_,Maggie,���N,Maisie,����,Mandy,�Ҹ�,Marcia,�����,Marcie,����,Margaret,�����R�S,Margery,���N��,Maria,���R��,Marian,���R�w,Mary,���R,Marilyn,�����Y,Marlene,���Y,Martha,����,Melanie,���ԩg,Mercedes,����w��,Mignon,���^,Mimi,����,Martha,����,Martina,���B�R,Mary,���R,Maud,���w,Maureen,���L,Mavis,������,Meg,����,Melanie,���ԥ�,Melinda,���L�F,Melissa,������,Michelle,�̳���,Mildred,�̺��w���w,Millicent,�̲��˯S,Millie,�̲�,Miranda,�����F,Miriam,�̨��ȩi,Moira,�����,Molly,����,Monica,�����d,Muriel,�[����,Minnie,�̩g,Nadine,�ǥ�,Nina,�g�R,Nadia,�ǭ}��,Nan,�n,Nancy,�n��,Naomi,������,Natalie,�Ƕ��,Natasha,�Ƕ��,Nell,����,Nellie,����,Nicky,����,Nicola,���j��,Nicole,�g�F��,Nora,�թ�,Norma,�պ�,Nita,�g�F,Olga,����,Olympia,���L�ǧJ,Olive,������,Olivia,������ܦ,Pam,���i,Pamela,��e��,Pat,���w,Patience,�ؿ���,Pancy,�ﵷ,Persis,�զ起,Prudence,���|����,Patricia,���A���,Patsy,�p��,Patti,����,Paula,����,Pauline,�i�Y,Pearl,�p��,Peggie,�خV,Penelope,�ؤ�����,Penny,�^��,Philippa,�����,Phoebe,���,Phyllis,��g��,Poll,�i��,Polly,�i�Q,Priscilla,�������,Pru,���|,Renee,�稺,Rachel,�p����,Rebecca,�R���d,Rhoda,ù�F,Rita,�R��,Roberta,ù�B��,Robin,ù��,Rosalie,ù���,Rosalind,ù��Y�w,Rosalyn,ù��Y,Rose,ù��,Rosemary,ù�����R,Rosie,ù��,Ruby,�S��,Rosa,ù��,Ruth,�S��,Salome,��ù��,Sylvia,�����,Sadie,�︦,Sal,�ﺸ,Sally,���,Sam,��i,Samantha,�İҲ�,Sandra,��w��,Sandy,�Ḧ,Sara,���i,Sharon,���@,Sheila,�Ʃ�,Sherry,����,Shirley,����,Sibyl,���i,Silvia,�����,Sonia,������,Sophia,Ĭ���,Sophronia,Ĭ��ֲ���,Sophie,����,Stella,�v�L��,Stephanie,�v����g,Susan,Ĭ��,Susanna,Ĭ�k�R,Sue,Ĭ,Susie,Ĭ��,Suzanne,Ĭ�k,Tabitha,�Ӥ��,Teresa,�S����,Terri,�S��,Tess,�S��,Tessa,�S��,Thelma,�뺸��,Tina,غ�R,Thalia,�ɩg��,Thea,�ɨ�,Thirza,���,Toni,�쥧,Tracy,�Z��,Tricia,�Z���,Trudie,�S�|�},Uerica,����,Una,�׮R,Undine,���L,Ursula,�̪L��,Urania,�׷�g��,Vivian,�O��,Vivien,������,Val,�˺�,Valerie,�˵ܧQ,Vanessa,�ˤ���,Vera,����,Veronica,���ԩg�d,Vicky,����,Victoria,���h�Q��,Viola,������,Violet,�˶��ӯS,Virginia,���N����,Viv,����,Willa,�®R,Wendy,�Ÿ�,Winifred,�¥����p�w,Winnie,�¥�,Yvonne,�춾�R,Zoe,����,������Y��,ʨ��M,�X�p�[�٧ڤ�,�ֺ�����,�M��,�N�t��,���B?��,�y�,����,�]�M��,�j�j,��,��,�B,��,�T,�س�,�ǳ�,����,,�Ӯ�,�ɧ�,Abe,����,Abel,�ȫ���,Abner,�ȧB��,Abraham,�ȧB�Ԩu,Allen,�ȭ�,Adam,�ȷ�,Adolf,�ȹD��,Albin,�㺸��,Alden,�㺸�y,Alexis,�㥧�ɴ�,Ambrose,�w���|��,Amos,�w����,Adrian,���w���w,Al,����,Albert,���B�S,Alexander,�Ⱦ��s�j,Alfred,�㺸���p�w,Alistair,���Q������,Alvin,������,Andrew,�w�w�|,Andy,�w�},Anselm,�w�h�i,Anthony,�w����,Antony,�w�F��,Angus,�w�洵,Archibald,���_�պ�,Archie,���_,Arnold,���ռw,Arthur,����,Augustin,���j���B,Augustus,���j����,Auberon,���B��,Aubrey,������,Baldwin,������,Bertran,�B�t,Bryan,�����w,Barnaby,�گǤ�,Barry,�ڨ�,Bartholomew,�ڶ묥�[,Basil,�ڤh��,Ben,�Z,Benjamin,�Z����,Bernard,�B�Ǽw,Bernie,�B��,Bert,�B�S,Bill,��,Billy,��Q,Bob,�j�B,Bobby,�ڤ�,Boris,�j����,Bradford,���Լw�I�o,Brad,���Լw,Brandon,�����y,Brendan,���ۤ�,Brian,���ܮ�,Bruce,���|��,Bud,�ڼw,Burt,����,Caesar,�ͼ�,Calvin,�d�Z,Carlton,�d���y,Cary,�d��,Christian,�J�����B,Carl,�d��,Cecil,��躸,Cedric,���w���J,Charles,�d�z�h,Charlie,��z,Chuck,�d�J,Christopher,�J�����h��,Chris,�J����,Clarence,�J�ԭ۴�,Clark,�J�ԧJ,Claude,�J�Ҽw,Clement,�J����,Clare,�J�ܺ�,Constant,�d���Z,Curtis,�F����,Clifford,�J���֯S,Cliff,�J�Q��,Clint,�J�L�S,Clive,�J�ܤ�,Clyde,�J�ܼw,Colin,��L,Craig,�J�p��,Curt,�_�S,Cyril,�訽��,Cuthbert,�d���B�S,Dexter,���J���S,Derby,�j��,Dale,����,Daniel,������,Dan,��,Danny,����,Darrell,�F�p��,Darren,�F��,David,�j��,Dave,����,Dean,�f��,Dennis,������,Derek,�w��J,Dermot,�w���S,Desmond,�w���X�o,Des,�w��,Dick,�f�J,Dirk,�w�J,Dominic,�h�̥��J,Donald,��Ǽw,Don,��,Douglas,�D��Դ�,Doug,�D��,Duane,���w,Dudley,�F�w�Q,Dud,�F�w,Duncan,�H��,Dustin,�F���B,Dwight,�w�h�S,Duke,���J,Earl,�R��,Ebenezer,�R�����d,Eamonn,�R�X,Ed,�R�},Edgar,�J�w�[,Edmund,�R�w�X,Edward,�R�w��,Edwin,�R�w��,Eliot,�㲤�S,Elmer,�R��,Elroy,�J��ù��,Emlyn,�J�i�L,Enoch,��էJ,Eric,�J���J,Ernest,�ڤ����S,Errol,�Jù��,Eugene,�ת�,Eli,�R�z,Enos,�̩�,Freddie,���p�},Felix,�O���J��,Ferdinand,�O�}�n�w,Fergus,���洵,Floyd,������w,Francis,�k���贵,Frank,�k���J,Frankie,������,Frederick,���p�w�p�J,Fred,���p�w,Gaston,�\���y,Gabriel,�\���纸,Gareth,�[�p��,Gary,�\��,Gavin,�[��,Gene,�N��,Geoffrey,�N����,Geoff,�N��,George,��v,Geraint,�N�ۯS,Gerald,�N�纸,Gerry,�樽,Gerard,�N�Ǽw,Gilbert,�N�B�S,Giles,�뺸��,Glen,���,Godfrey,�ॱ�p,Gordon,��n,Graham,��p�̩i,Graeme,��p�i,Gregory,��p�਽,Greg,��p��,Guy,�\��,Gideon,�N�a��,Grant,�����w,Humphry,�~����,Hal,����,Hank,�~�J,Harold,���o�w,Harry,���Q,Henry,��Q,Herbert,���B�S,Horace,�N�Ǵ�,Howard,�N�ؼw,Hubert,��B�S,Hugh,��,Hamilton,�~�̺��y,Hector,���J�S,Heman,����,Hugo,�B�G,Herman,����,Hilary,�ƩԷ�,Howell,�N�º�,Hugh,��,Humphrey,����,Hiram,����,Homer,����,Ian,�쮦,Isaac,���ħJ,Ivan,��Z,Ivor,�㥱,Ira,�J��,Irving,�J��,Irwin,�J��,Jarvis,�����,Jean,�Ǯ�,Job,�ŧB,Jack,�ǧJ,Jacob,�Ǭ_,Jake,�N�J,James,��i�h,Jamie,�N��,Jason,�ǥ�,Jasper,�봵��,Jed,�N�w,Jeff,�Ǥ�,Jeffrey,�Ǥҷ�,Jeremy,�N����,Jerome,�Nù�i,Jerry,�N��,Jesse,�N��,Jessy,�Ǧ�,Jim,�N�i,Jimmy,�N��,Jock,��J,Joe,��,John,����,Johnny,�j��,Jonathan,��Ǵ�,Jon,�쮦,Joseph,�����,Joshua,���Ѩ�,Julian,���Q�w,Justin,�봵�B,Julius,�Ѳz��,Karl,�d��,Kay,�J,Keith,�J�h,Ken,��,Kenneth,�֥���,Kenny,�֥�,Kent,�֯S,Kevin,�ͤ�,Kit,�ͯS,Kev,�ͤ�,Kirk,�_�J,Laban,�p�Z,Lee,��,Lance,����,Larry,��Q,Laurence,�ҭ۴�,Len,�ܮ�,Lenny,�ۥ�,Leo,����,Leonard,���Ǽw,Les,�ܴ�,Leslie,�ܴ��Q,Lester,�ܴ��S,Lew,�c,Leon,����,Lincoln,�L��,Lewis,�B���h,Liam,�Q�ȩi,Lionel,�ܩ�����,Lou,��,Louis,�����h,Luke,���J,Lucius,���ߴ�,Luman,�`��,Lynn,�L,Malcolm,�����J�i,Mark,���J,Martin,���B,Malachi,�ک԰�,Marshall,������,Marvin,����,Marty,����,Matt,���S,Mattew,����,Matlhew,�ڪL,Milton,�����y,Monroe,�X��,Maurice,������,Max,���J��,Mervyn,�q��,Michael,���i,Mick,�̧J,Micky,�̰�,Miles,�ں���,Mike,���J,Mitch,�̩_,Mitchell,�̤���,Morris,������,Mort,���S,Murray,����,Morgan,����,Montgomery,�X������,Na'amai,�Ǫ���,Nat,�ǯS,Nathan,����,Nahum,�p��,Napoleon,���}�[,Nelson,�Ǻ���,Newton,�b�y,Noah,�ը�,Norbert,�պ��B,Nathaniel,�Ǽ�����,Neal,����,Ned,���o,Neddy,���a,Nicholas,���i�Դ�,Nick,���J,Nicky,����,Nigel,�`�N��,Noel,�պ�,Norm,�թi,Norman,�պ���,Ollie,���Q,Oliver,���Q��,Oscar,�����d,Oswald,�����˺��w,Owen,�ڤ�,Oz,����,Ozzie,����,Octavius,���J�S��,Osmond,������,Otto,�k��,Paddy,���},Pat,���S,Patrick,���A�J,Paul,�Où,Percy,�Ħ�,Pete,�֯S,Peter,���o,Phil,�Ẹ,Philip,��O�Z,Padraic,�i��N,Pearce,�ֺ���,Perry,�B�z,Philander,�����w,Philemen,��z�X,Pius,������,Quentin,����,Quincy,����,Rene,��p,Reuben,�|��,Ralph,�Ժ���,Randolf,�۹D��,Randy,���},Raphael,�Դ���,Ray,�p,Raymond,�p�X,Reg,�p�N,Reginald,�p���Ǽw,Rex,�p�J��,Richard,�z��,Richie,���_,Rick,���J,Ricky,����,Rob,ù��,Robbie,ù��,Robby,�դ�,Robert,ù�k,Robin,ù��,Rod,ù�w,Roderick,ù�w���J,Rodney,ù�w��,Rodge,ù��,Roger,ù��,Ronald,ù�ռw,Ron,ù��,Ronnie,�s��,Rory,ù��,Roy,ù��,Rudolph,�|�D��,Rufus,�|����,Rupert,�|�įS,Russ,�Դ�,Reuel,�|��,Reynold,�p��,Roland,ù���w,Ross,ù��,Russell,ù��,Samson,���i��,Saul,����,Sam,�s�i,Sammy,�Ħ�,Samuel,�s�պ�,Sandy,�s��,Scott,�q�U��,Seamas,���q��,Sean,�v��,Seb,�륬,Sebastian,��ڴ��Ҧw,Sid,���w,Sidney,�x��,Simon,��X,Stan,���Z,Stanley,���Z��,Steve,�v����,Steven,�v����,Stewart,�v����,Sinclair,���J��,Solomon,��ù��,Stuart,�v����,Ted,���w,Teddy,���w,Tel,�S��,Terence,�S�۴�,Terry,�o�Q,Theo,���,Theodore,����h,Thomas,������,Tim,�ҩi,Timmy,����,Timothy,�Ҳ���,Toby,���,Tom,���i,Tommy,����,Tony,�F��,Theobald,�f���_�S,Theodoric,�f���h�z,Terence,���۴�,Trevor,�S�p��,Troy,�S����,Urban,�Q���Z,Van,�S,Vivian,�����w,Vic,���J,Victor,���J��,Vince,�崵,Vincent,��˯S,Viv,����,Wallace,�صܤh,Wally,�U�Q,Walter,�غ��S,Warren,�ح�,Wayne,����,Wesley,�����z,Winston,�Ŵ���,Will,�º�,Wilbur,�º��B,Wilfred,�º������w,Willy,�§Q,William,�·G,Willis,�§Q��";
		nickArray = names.Split(',');
		int len = nickArray.Length;
		System.Random random = new System.Random();
		int index = random.Next(len);
		string str = nickArray[index];
		return str;
	}
	public void CloseGame()
	{
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
		Debug.Log(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
#endif
#if (UNITY_EDITOR)
		UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_STANDALONE)
            Application.Quit();
#elif (UNITY_WEBGL)
            Application.OpenURL("about:blank");
#endif
	}

	public void SetLevel(int level = 1)
	{
		mNowLevel = level;
		switch (level)
		{
			case 3:
				mBounsTimeScore = mBounsTimeScore + 150;
				MonsterNameText.text = MonsterNames[2];
				MonsterU.IdelAnim = Level3_Monster;
				mMonsterMaxHP = NowMonsterHP = L3_HP;
				break;

			case 2:
				mBounsTimeScore = mBounsTimeScore + 100;
				MonsterNameText.text = MonsterNames[1];
				MonsterU.IdelAnim = Level2_Monster;
				mMonsterMaxHP = NowMonsterHP = L2_HP;
				break;

			case 1:
			default:
				sw.Reset();//�X���k�s
				sw.Start();//�X���}�l�p��
				IsGameOver = false;
				mBounsTimeScore = 300;
				MonsterNameText.text = MonsterNames[0];
				MonsterU.IdelAnim = Level1_Monster;
				NowMonsterHP = mMonsterMaxHP = L1_HP;
				break;
		}

		MonsterU.AnimControl();
		ChangeMonster = false;
		PlayerAttack(0);
	}
	bool ChangeMonster = false;
	bool IsGameOver = false;
	public void PlayerAttack(int hp)
	{
		NowMonsterHP = NowMonsterHP - hp;
		NowMonsterHP = Mathf.Max(NowMonsterHP, 0);
		DOTween.To(() => MonsterHPBar.fillAmount, x => MonsterHPBar.fillAmount = x, ((float)NowMonsterHP / (float)mMonsterMaxHP), 0.2f).OnComplete(() =>
		{
			if (NowMonsterHP > 0 && hp > 0)
			{
				MonsterTrans.localPosition = new Vector3(0, -85, 0);
				mPosTweener = MonsterTrans.DOPunchPosition(Vector3.one * 10f, 0.5f, 30).OnComplete(() => MonsterTrans.localPosition = new Vector3(0, -85, 0));

				MonsterU.TargetImage.color = Color.white;
				mColorTweener = MonsterU.TargetImage.DOColor(Color.red, 0.05f).SetLoops(2, LoopType.Yoyo).OnComplete(() => { MonsterU.TargetImage.color = Color.white; });
				var hpbar = HPList.Find(h => { return !h.gameObject.activeInHierarchy; });

				if (hpbar == null)
				{
					hpbar = GameObject.Instantiate(HPPrefab, HPRoot);
					HPList.Add(hpbar);
				}

				float lx = UnityEngine.Random.Range(-150, 150);
				float ly = UnityEngine.Random.Range(200, 250);
				hpbar.localPosition = new Vector3(lx, ly, 0);
				hpbar.gameObject.SetActive(true);
				var textPro = hpbar.GetComponent<TextMeshProUGUI>();
				textPro.color = Color.red;
				textPro.text = hp.ToString();
				hpbar.DOLocalMoveY(330, 1).OnComplete(() =>
				{
					hpbar.gameObject.SetActive(false);
				});
				textPro.DOColor(new Color(0, 0, 0, 0), .9f).Play();
				SetScore(hp * 10);
				//DOTween.To(() => Score, x => Score = x, Score + hp * 10, 0.2f);
			}
			if (NowMonsterHP <= 0)
			{
				PlaySound(DamSound);
				ChangeMonster = true;
				SetScore(mNowLevel * 1000 + ((int)mBounsTimeScore * 10));
				//DOTween.To(() => Score, x => Score = x, Score + mNowLevel * 1000 + ((int)mBounsTimeScore * 10), 0.2f);
				if (mNowLevel < 3)
				{
					SetLevel(mNowLevel + 1);
				}
				else
				{
					if (IsGameOver)
						return;

					IsGameOver = true;
				}
			}
		});
	}

	private void GameOver()
	{
		sw.Stop();
		//SQLServer.SendData(UserName, Score);

		//SQLServer.GetTop5((data) =>
		//{
		//    if (data != null)
		//    {
		//        for (int i = 0; i < Ranks.Count; i++)
		//        {
		//            Ranks[i].SetData(data[i]);
		//        }
		//    }
		//});
		WinTitleMarquee.text = MarqueeStrs[UnityEngine.Random.Range(0, 3)];
		MarqueeRoot.SetActive(true);
		WinTitleMarquee.rectTransform.DOMoveX(0, 1f).OnComplete(() => { });
		GameOverPage.SetActive(true);
	}

	private void SetScore(int addSocre)
	{
		if (IsGameOver == false)
		{
			DOTween.To(() => Score, x => Score = x, Score + addSocre, 0.2f).OnComplete(() =>
			{
				if (IsGameOver)
					GameOver();
			});
		}
	}

	private void StartVideo_prepareCompleted(VideoPlayer source)
	{
		Debug.Log($"Load Done");
		LogoUIBtn.gameObject.SetActive(true);
		m_StartBtn.gameObject.SetActive(true);
	}

	public string CheckInputText(string str)
	{
		System.Text.StringBuilder sb = new System.Text.StringBuilder(str.Length);
		string filterText = "�F|��|�A�Q|Fuck|���k|����|�u�j";

		string[] filterData = filterText.Split('|');
		Dictionary<char, List<string>> dicList = new Dictionary<char, List<string>>();
		foreach (var item in filterData)
		{
			char value = item[0];
			if (dicList.ContainsKey(value))
				dicList[value].Add(item);
			else
				dicList.Add(value, new List<string>() { item });
		}

		int count = str.Length;
		for (int i = 0; i < count; i++)
		{
			char word = str[i];
			if (dicList.ContainsKey(word))
			{
				int num = 0;
				var data = dicList[word].OrderBy(g => g.Length);

				foreach (var wordbook in data)
				{
					if (i + wordbook.Length <= count)
					{
						string result = str.Substring(i, wordbook.Length);
						if (result == wordbook)
						{
							num = 1;
							sb.Append(GetString(result));
							i = i + wordbook.Length - 1;//��?���\ �P?��?i������
							break;
						}
					}
				}
				if (num == 0)
					sb.Append(word);
			}
			else
				sb.Append(word);
		}
		return sb.ToString();
	}

	private static string GetString(string value)
	{
		string starNum = string.Empty;
		for (int i = 0; i < value.Length; i++)
		{
			starNum += "*";
		}
		return starNum;
	}
}
