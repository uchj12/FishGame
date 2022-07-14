using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class start : MonoBehaviourPunCallbacks
{
    [SerializeField, Tooltip("���[��������̍ő�v���[���[��")]
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
        // ����ɂ��A�}�X�^�[�N���C�A���g��PhotonNetwork.LoadLevel�i�j��
        // �g�p�ł���悤�ɂȂ�A�������[���ɂ��邷�ׂẴN���C�A���g�����x���������I�ɓ������܂�
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
            // ���̎��_�ŁA�����_�����[���ɎQ������K�v������܂��B
            // ���s�����ꍇ�́AOnJoinRandomFailed�i�j�Œʒm���󂯎��A�쐬���܂�
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
        Debug.Log("PUN/�����`���[�FOnJoinRandomFailed�i�j��PUN�ɂ���ČĂяo����܂����B ���p�ł��郉���_���ȃ��[�����Ȃ����߁A�쐬���܂��B\nCalling�FPhotonNetwork.CreateRoom ");

        // #Critical
        // �����_���ȃ��[���ɎQ���ł��܂���ł����B
        // ���݂��Ȃ����A�S���������ł���\��������܂��B �S�z����܂���A�V�������[�������܂��B 
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = m_maxPlayersPerRoom });
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            Debug.Log("PUN/�����`���[�FPUN�ɂ���ČĂяo�����OnJoinedRoom�i�j�B ���A���̃N���C�A���g�̓��[���ɂ��܂��B ");

            // #Critical
            // �ŏ��̃v���[���[�ł���ꍇ�ɂ̂݃��[�h���܂��B
            // ����ȊO�̏ꍇ�́A�C���X�^���X�V�[���𓯊����邽�߂�
            // `PhotonNetwork.AutomaticallySyncScene`�Ɉˑ����܂��B
                Debug.Log("'main'�����[�h���܂�");

                // #Critical
                // ���[�����x�������[�h���܂��B
                PhotonNetwork.LoadLevel("main");
            
        }
        else
        {
            Debug.Log("�l����������Ă��܂��� ");
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        JoinRandomRoomPanel.SetActive(true);
        MachingPanel.SetActive(false);
    }



}
