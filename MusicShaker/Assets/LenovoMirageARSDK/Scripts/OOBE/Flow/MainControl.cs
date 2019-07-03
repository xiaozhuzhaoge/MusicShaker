namespace LenovoMirageARSDK.OOBE
{
    public class MainControl: Entity
	{
		//--			
		public enum Transtion
		{
			isKey,
			FirstGame,
			Back,
			SecondGame,
			Enter,
			HintDown,
			SettingEnd,
			isKey1,
			Ctr,
			Usb,
			Beacon,
			Start,
			Buy,
		}//--
		public MainControl() : base()
		{

		}

		public MainControl(object m_Obj) : base(m_Obj)
		{

		}


        protected override void InitState()
		{			
			//**
			SplashState m_SplashState= new SplashState(this);
			m_SplashState.AddTransition("FirstGame","WelcomeState");
			m_SplashState.AddTransition("SecondGame","SettingHint");
			AddState(m_SplashState);
						
			//**
			WelcomeState m_WelcomeState= new WelcomeState(this);
			m_WelcomeState.AddTransition("Enter","SettingHint");
			m_WelcomeState.AddTransition("Back","ExitFlowState");
			m_WelcomeState.AddTransition("Buy","ByState");
			AddState(m_WelcomeState);
						
			//**
			ExitFlowState m_ExitFlowState= new ExitFlowState(this);
			AddState(m_ExitFlowState);
						
			//**
			SettingHint m_SettingHint= new SettingHint(this);
			m_SettingHint.AddTransition("HintDown","DownApp");
			m_SettingHint.AddTransition("SettingEnd","StateGame");
			m_SettingHint.AddTransition("Ctr","ControllState");
			m_SettingHint.AddTransition("Usb","UsbState");
			m_SettingHint.AddTransition("Beacon","BeaconState");
			m_SettingHint.AddTransition("Back","ExitFlowState");
			AddState(m_SettingHint);
						
			//**
			DownApp m_DownApp= new DownApp(this);
			AddState(m_DownApp);
						
			//**
			StateGame m_StateGame= new StateGame(this);
			m_StateGame.AddTransition("Start","InPhone");
			AddState(m_StateGame);
						
			//**
			ControllState m_ControllState= new ControllState(this);
			m_ControllState.AddTransition("Usb","UsbState");
			m_ControllState.AddTransition("Beacon","BeaconState");
			m_ControllState.AddTransition("Back","SettingHint");
			AddState(m_ControllState);
						
			//**
			UsbState m_UsbState= new UsbState(this);
			m_UsbState.AddTransition("Back","SettingHint");
			m_UsbState.AddTransition("Ctr","ControllState");
			m_UsbState.AddTransition("Beacon","BeaconState");
			AddState(m_UsbState);
						
			//**
			BeaconState m_BeaconState= new BeaconState(this);
			m_BeaconState.AddTransition("Back","SettingHint");
			m_BeaconState.AddTransition("Ctr","ControllState");
			m_BeaconState.AddTransition("Usb","UsbState");
			AddState(m_BeaconState);
						
			//**
			InPhone m_InPhone= new InPhone(this);
			AddState(m_InPhone);
						
			//**
			ByState m_ByState= new ByState(this);
			AddState(m_ByState);
			//**
		}

		protected override void OnDestroy()
		{

		}

	}
}