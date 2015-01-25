using UnityEngine;
using ExitGames.Client.Photon;

public class GameManager : Photon.MonoBehaviour {
	static private GameManager instance = null;
	static public GameManager Instance {
		get {
			return GetInstance();
		}
	}
	static public GameManager GetInstance() {
		if( instance == null ) {
			instance = FindObjectOfType<GameManager>();
			if( instance ) return instance;
			GameObject obj = new GameObject( "GameManager" );
			instance = obj.AddComponent<GameManager>();

		}
		return instance;
	}

	void Awake() {
		instance = this;
	}

	void Start() {
		this.gameObject.AddComponent<ConnectAndJoinRandom>();
		DontDestroyOnLoad( gameObject );
	}

	public string nextScene {
		get;
		set;
	}

	public PhotonView view{
		get;
		set;
	}

	public int playercnt {
		get;
		set;
	}
	public int sceneid = -1;

	public void StageClear() {
		Debug.Log( "Clear" );
		SendStageClear();
	}

	public void StageChange() {
		if( sceneid != -1 ) {
			Debug.Log( "load" );
			PhotonNetwork.LoadLevel( sceneid );
			sceneid = -1;

			PhotonNetwork.isMessageQueueRunning = true;


			return;
		}
		PhotonNetwork.LoadLevel( nextScene );
		PhotonNetwork.isMessageQueueRunning = true;
		return;
	}

	void SendStageClear() {
		view.RPC( "RecvStageClear", PhotonTargets.All );
	}

	public void StageClear( string _name ) {
		nextScene = _name;
		sceneid = -1;
		StageClear();
	}
	public void StageClear( int id ) {
		sceneid = id;
		nextScene = "";
		StageClear();
	}

	public string ConnectLog {
		get;
		set;
	}

    public virtual void OnConnectedToMaster()
    {
		ConnectLog = "サーバーに接続成功 ロビーに接続中...";
	}

    public virtual void OnPhotonRandomJoinFailed()
    {
		ConnectLog = "joinに失敗";
	}

    // the following methods are implemented to give you some context. re-implement them as needed.

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
		ConnectLog = "Cause: " + cause;
    }

    public void OnJoinedRoom()
    {
		ConnectLog = "roomに接続成功";
	}

    public void OnJoinedLobby()
    {
		ConnectLog = "ロビーに接続成功 roomに接続中...";
	}

}
