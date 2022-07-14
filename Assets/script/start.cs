using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class start : MonoBehaviourPunCallbacks
{
    [SerializeField, Tooltip("ルームあたりの最大プレーヤー数")]
    private byte m_maxPlayersPerRoom = 2;

    [Header("Login Panel")]
    public GameObject LoginPanel;


    [Header("Join Random Room Panel")]
    public GameObject JoinRandomRoomPanel;

    [Header("Maching Room Panel")]
    public GameObject MachingPanel;

    bool m_isConnecting;
    RoomOptions roomOptions = new RoomOptions();

    void Awake()
    {
        // #Critical
        // これにより、マスタークライアントでPhotonNetwork.LoadLevel（）を
        // 使用できるようになり、同じルームにいるすべてのクライアントがレベルを自動的に同期します
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    void Start()
    {
        JoinRandomRoomPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }

    public void OnLoginButton()
    {
        m_isConnecting = PhotonNetwork.ConnectUsingSettings();
        JoinRandomRoomPanel.SetActive(true);
        LoginPanel.SetActive(false);
    }

    public void OnjoinRandomRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            // #Critical
            // この時点で、ランダムルームに参加する必要があります。
            // 失敗した場合は、OnJoinRandomFailed（）で通知を受け取り、作成します
            PhotonNetwork.JoinRandomRoom();
            JoinRandomRoomPanel.SetActive(false);
            MachingPanel.SetActive(true);
        }
        else
        {
            m_isConnecting = PhotonNetwork.ConnectUsingSettings();
            m_isConnecting = false;
        }
        //SceneManager.LoadScene("main");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN/ランチャー：OnJoinRandomFailed（）はPUNによって呼び出されました。 利用できるランダムなルームがないため、作成します。\nCalling：PhotonNetwork.CreateRoom ");

        // #Critical
        // ランダムなルームに参加できませんでした。
        // 存在しないか、全員が満員である可能性があります。 心配いりません、新しいルームを作ります。 
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = m_maxPlayersPerRoom });
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            Debug.Log("PUN/ランチャー：PUNによって呼び出されるOnJoinedRoom（）。 今、このクライアントはルームにいます。 ");

            // #Critical
            // 最初のプレーヤーである場合にのみロードします。
            // それ以外の場合は、インスタンスシーンを同期するために
            // `PhotonNetwork.AutomaticallySyncScene`に依存します。
                Debug.Log("'main'をロードします");

                // #Critical
                // ルームレベルをロードします。
                PhotonNetwork.LoadLevel("main");
            
        }
        else
        {
            Debug.Log("人数がそろっていません ");
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        JoinRandomRoomPanel.SetActive(true);
        MachingPanel.SetActive(false);
    }



}
