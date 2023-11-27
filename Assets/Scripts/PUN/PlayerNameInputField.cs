using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMPro.TMP_InputField))]
public class PlayerNameInputField : MonoBehaviour
{
    static string playerNamePrefKeys = "PlayerName";

    private void Start()
    {
        string defaultName = string.Empty;

        if (TryGetComponent<InputField>(out InputField inputField))
        {
            if (PlayerPrefs.HasKey(playerNamePrefKeys))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKeys);
                inputField.text = defaultName;
            }
        }

        PhotonNetwork.NickName = defaultName;
    }

    public void SetPlayerName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.Log("Name is null or empty");
            return;
        }
        PhotonNetwork.NickName = name;
        PlayerPrefs.SetString(playerNamePrefKeys, name);
    }
}
