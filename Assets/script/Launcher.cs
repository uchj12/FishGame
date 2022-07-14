using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// 他のアセットや開発者との競合を防ぐ
namespace Com.MyCompany.MyGame
{
    /// <summary>
    /// マネージャーを起動します。
    /// 接続するか、ランダムなルームに参加するか、
    /// ルームがないかすべてがいっぱいの場合はルームを作成します。 
    /// </summary>
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        [SerializeField, Tooltip("ルームあたりの最大プレーヤー数")]
        private byte m_maxPlayersPerRoom = 4;

        [SerializeField, Tooltip("ユーザーが名前を入力し、接続して再生できるようにするUIパネル ")]
        private GameObject m_controlPanel;

        [SerializeField, Tooltip("接続が進行中であることをユーザーに通知するためのUIラベル ")]
        private GameObject m_progressLabel;

        #endregion

        #region Private Fields

        /// <summary>
        /// このクライアントのバージョン番号。
        /// ユーザーはgameVersionによって互いに分離されています
        /// （これにより、重大な変更を加えることができます）
        /// </summary>
        string m_gameVersion = "1";

        /// <summary>
        /// 現在のプロセスを追跡します。 接続は非同期であり、Photonからのいくつかのコールバックに基づいているため、 
        /// Photonからコールバックを受け取ったときの動作を適切に調整するには、これを追跡する必要があります。 
        /// 通常、これはOnConnectedToMaster（）コールバックに使用されます。 
        /// </summary>
        bool m_isConnecting;

        #endregion

        #region MonoBehaviour CallBacks

        void Awake()
        {
            // #Critical
            // これにより、マスタークライアントでPhotonNetwork.LoadLevel（）を
            // 使用できるようになり、同じルームにいるすべてのクライアントがレベルを自動的に同期します
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        void Start()
        {
            m_progressLabel.SetActive(false);
            m_controlPanel.SetActive(true);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 接続プロセスを開始します。
        /// -すでに接続されている場合は、ランダムなルームに参加しようとします
        /// -まだ接続されていない場合は、このアプリケーションインスタンスをPhoton CloudNetworkに接続します
        /// </summary>
        public void Connect()
        {
            m_progressLabel.SetActive(true);
            m_controlPanel.SetActive(false);
            // 接続されているかどうかを確認し、接続されている場合は参加します。
            // 接続されていない場合は、サーバーへの接続を開始します。 
            if (PhotonNetwork.IsConnected)
            {
                // #Critical
                // この時点で、ランダムルームに参加する必要があります。
                // 失敗した場合は、OnJoinRandomFailed（）で通知を受け取り、作成します
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical
                // まず第一に、Photon OnlineServerに接続する必要があります。
                // ゲームから戻ったときに接続されているコールバックを受け取るため、

                // 部屋に参加する意志を追跡します。
                // そのため、次に何をすべきかを知る必要があります。
                m_isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = m_gameVersion;
            }
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("PUNの基本チュートリアル/ランチャー：OnConnectedToMaster（）はPUNによって呼び出されました ");

            // #Critical
            // ランダムなルームに参加できませんでした。
            // 存在しないか、全員が満員である可能性があります。
            // その場合は新しいルームを作ります。 
            //PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = m_maxPlayersPerRoom });

            // 部屋に参加しようとしない限り、何もしません。
            // isConnectingがfalseであるこのケースは、通常、ゲームに負けた、またはゲームを終了したときです。
            // このレベルがロードされると、OnConnectedToMasterが呼び出されます。その場合は何もしません。 
            if (m_isConnecting)
            {
                // #Critical
                // 私たちが最初にやろうとしているのは、潜在的な既存の部屋に参加することです。
                // それ以外の場合は、OnJoinRandomFailed（）でコールバックされます。 
                PhotonNetwork.JoinRandomRoom();
                m_isConnecting = false;
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN/ランチャー：OnJoinRandomFailed（）はPUNによって呼び出されました。 利用できるランダムなルームがないため、作成します。\nCalling：PhotonNetwork.CreateRoom ");

            // #Critical
            // ランダムなルームに参加できませんでした。
            // 存在しないか、全員が満員である可能性があります。 心配いりません、新しいルームを作ります。 
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = m_maxPlayersPerRoom });
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN/ランチャー：OnDisconnected（）が理由{0}でPUNによって呼び出されました ", cause);
            m_progressLabel.SetActive(false);
            m_controlPanel.SetActive(true);
            m_isConnecting = false;
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN/ランチャー：PUNによって呼び出されるOnJoinedRoom（）。 今、このクライアントはルームにいます。 ");

            // #Critical
            // 最初のプレーヤーである場合にのみロードします。
            // それ以外の場合は、インスタンスシーンを同期するために
            // `PhotonNetwork.AutomaticallySyncScene`に依存します。
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("'Room1'をロードします");

                // #Critical
                // ルームレベルをロードします。
                PhotonNetwork.LoadLevel("Room1");
            }
        }

        #endregion
    }
}