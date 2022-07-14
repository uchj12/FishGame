using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// ���̃A�Z�b�g��J���҂Ƃ̋�����h��
namespace Com.MyCompany.MyGame
{
    /// <summary>
    /// �}�l�[�W���[���N�����܂��B
    /// �ڑ����邩�A�����_���ȃ��[���ɎQ�����邩�A
    /// ���[�����Ȃ������ׂĂ������ς��̏ꍇ�̓��[�����쐬���܂��B 
    /// </summary>
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        [SerializeField, Tooltip("���[��������̍ő�v���[���[��")]
        private byte m_maxPlayersPerRoom = 4;

        [SerializeField, Tooltip("���[�U�[�����O����͂��A�ڑ����čĐ��ł���悤�ɂ���UI�p�l�� ")]
        private GameObject m_controlPanel;

        [SerializeField, Tooltip("�ڑ����i�s���ł��邱�Ƃ����[�U�[�ɒʒm���邽�߂�UI���x�� ")]
        private GameObject m_progressLabel;

        #endregion

        #region Private Fields

        /// <summary>
        /// ���̃N���C�A���g�̃o�[�W�����ԍ��B
        /// ���[�U�[��gameVersion�ɂ���Č݂��ɕ�������Ă��܂�
        /// �i����ɂ��A�d��ȕύX�������邱�Ƃ��ł��܂��j
        /// </summary>
        string m_gameVersion = "1";

        /// <summary>
        /// ���݂̃v���Z�X��ǐՂ��܂��B �ڑ��͔񓯊��ł���APhoton����̂������̃R�[���o�b�N�Ɋ�Â��Ă��邽�߁A 
        /// Photon����R�[���o�b�N���󂯎�����Ƃ��̓����K�؂ɒ�������ɂ́A�����ǐՂ���K�v������܂��B 
        /// �ʏ�A�����OnConnectedToMaster�i�j�R�[���o�b�N�Ɏg�p����܂��B 
        /// </summary>
        bool m_isConnecting;

        #endregion

        #region MonoBehaviour CallBacks

        void Awake()
        {
            // #Critical
            // ����ɂ��A�}�X�^�[�N���C�A���g��PhotonNetwork.LoadLevel�i�j��
            // �g�p�ł���悤�ɂȂ�A�������[���ɂ��邷�ׂẴN���C�A���g�����x���������I�ɓ������܂�
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
        /// �ڑ��v���Z�X���J�n���܂��B
        /// -���łɐڑ�����Ă���ꍇ�́A�����_���ȃ��[���ɎQ�����悤�Ƃ��܂�
        /// -�܂��ڑ�����Ă��Ȃ��ꍇ�́A���̃A�v���P�[�V�����C���X�^���X��Photon CloudNetwork�ɐڑ����܂�
        /// </summary>
        public void Connect()
        {
            m_progressLabel.SetActive(true);
            m_controlPanel.SetActive(false);
            // �ڑ�����Ă��邩�ǂ������m�F���A�ڑ�����Ă���ꍇ�͎Q�����܂��B
            // �ڑ�����Ă��Ȃ��ꍇ�́A�T�[�o�[�ւ̐ڑ����J�n���܂��B 
            if (PhotonNetwork.IsConnected)
            {
                // #Critical
                // ���̎��_�ŁA�����_�����[���ɎQ������K�v������܂��B
                // ���s�����ꍇ�́AOnJoinRandomFailed�i�j�Œʒm���󂯎��A�쐬���܂�
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical
                // �܂����ɁAPhoton OnlineServer�ɐڑ�����K�v������܂��B
                // �Q�[������߂����Ƃ��ɐڑ�����Ă���R�[���o�b�N���󂯎�邽�߁A

                // �����ɎQ������ӎu��ǐՂ��܂��B
                // ���̂��߁A���ɉ������ׂ�����m��K�v������܂��B
                m_isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = m_gameVersion;
            }
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN�̊�{�`���[�g���A��/�����`���[�FOnConnectedToMaster�i�j��PUN�ɂ���ČĂяo����܂��� ");

            // #Critical
            // �����_���ȃ��[���ɎQ���ł��܂���ł����B
            // ���݂��Ȃ����A�S���������ł���\��������܂��B
            // ���̏ꍇ�͐V�������[�������܂��B 
            //PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = m_maxPlayersPerRoom });

            // �����ɎQ�����悤�Ƃ��Ȃ�����A�������܂���B
            // isConnecting��false�ł��邱�̃P�[�X�́A�ʏ�A�Q�[���ɕ������A�܂��̓Q�[�����I�������Ƃ��ł��B
            // ���̃��x�������[�h�����ƁAOnConnectedToMaster���Ăяo����܂��B���̏ꍇ�͉������܂���B 
            if (m_isConnecting)
            {
                // #Critical
                // ���������ŏ��ɂ�낤�Ƃ��Ă���̂́A���ݓI�Ȋ����̕����ɎQ�����邱�Ƃł��B
                // ����ȊO�̏ꍇ�́AOnJoinRandomFailed�i�j�ŃR�[���o�b�N����܂��B 
                PhotonNetwork.JoinRandomRoom();
                m_isConnecting = false;
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN/�����`���[�FOnJoinRandomFailed�i�j��PUN�ɂ���ČĂяo����܂����B ���p�ł��郉���_���ȃ��[�����Ȃ����߁A�쐬���܂��B\nCalling�FPhotonNetwork.CreateRoom ");

            // #Critical
            // �����_���ȃ��[���ɎQ���ł��܂���ł����B
            // ���݂��Ȃ����A�S���������ł���\��������܂��B �S�z����܂���A�V�������[�������܂��B 
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = m_maxPlayersPerRoom });
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN/�����`���[�FOnDisconnected�i�j�����R{0}��PUN�ɂ���ČĂяo����܂��� ", cause);
            m_progressLabel.SetActive(false);
            m_controlPanel.SetActive(true);
            m_isConnecting = false;
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN/�����`���[�FPUN�ɂ���ČĂяo�����OnJoinedRoom�i�j�B ���A���̃N���C�A���g�̓��[���ɂ��܂��B ");

            // #Critical
            // �ŏ��̃v���[���[�ł���ꍇ�ɂ̂݃��[�h���܂��B
            // ����ȊO�̏ꍇ�́A�C���X�^���X�V�[���𓯊����邽�߂�
            // `PhotonNetwork.AutomaticallySyncScene`�Ɉˑ����܂��B
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("'Room1'�����[�h���܂�");

                // #Critical
                // ���[�����x�������[�h���܂��B
                PhotonNetwork.LoadLevel("Room1");
            }
        }

        #endregion
    }
}