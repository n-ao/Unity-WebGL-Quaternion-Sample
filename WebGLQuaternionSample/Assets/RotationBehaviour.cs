using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// 振る舞いクラス
/// </summary>
public class RotationBehaviour : MonoBehaviour
{
    /// <summary>
    /// カメラ
    /// </summary>
    [SerializeField] Camera _sceneCamera;
    /// <summary>
    /// 動かす対象オブジェクト
    /// </summary>
    [SerializeField] GameObject _target;

    #region UI
    /// <summary>
    /// X座標に対応するテキストラベル
    /// </summary>
    [SerializeField] Text _textValueX;
    /// <summary>
    /// Y座標に対応するテキストラベル
    /// </summary>
    [SerializeField] Text _textValueY;
    /// <summary>
    /// Z座標に対応するテキストラベル
    /// </summary>
    [SerializeField] Text _textValueZ;

    /// <summary>
    /// ローカル回転かどうかのスイッチ
    /// </summary>
    [SerializeField] Toggle _isLocalSwitch;
    /// <summary>
    /// 段階的に回転させるかのスイッチ
    /// </summary>
    [SerializeField] Toggle _isStepSwitch;
    /// <summary>
    /// 自動回転させるかのスイッチ
    /// </summary>
    [SerializeField] Toggle _isAutoRotateSwitch;
    #endregion

    #region private fields
    [SerializeField] private float _valueX = 0.0f;
    [SerializeField] private float _valueY = 0.0f;
    [SerializeField] private float _valueZ = 0.0f;
    [SerializeField] private float _addingvalue = 1.0f;
    [SerializeField] private bool _isLocal = false;
    [SerializeField] private bool _isStep = false;
    [SerializeField] private bool _isAutoRotate = false;
    [SerializeField] private bool _isCSharpImplementedQaternion = false;
    #endregion

    #region RotationAxis
    [SerializeField] RotationAxis _currenRotationAxis = RotationAxis.X;
    private enum RotationAxis
    {
        X,
        Y,
        Z
    }
    #endregion

    #region Event
    [SerializeField] private UnityEvent _valueChangedEvent;
    #endregion

    #region Unity Methods
    private void Start()
    {
        // 初期化
        _isLocalSwitch.isOn = _isLocal;
        _isStepSwitch.isOn = _isStep;
        _isAutoRotateSwitch.isOn = _isAutoRotate;
        RotateByQuaternion(0f, 0f, 0f, _target, _isLocal, _isStep);

        // リスナー紐付け
        _isLocalSwitch.onValueChanged.AddListener(OnLocalSwitchValueChanged);
        _isStepSwitch.onValueChanged.AddListener(OnStepSwitchValueChanged);
        _isAutoRotateSwitch.onValueChanged.AddListener(OnAutoRotateValueChanged);
        _valueChangedEvent.AddListener(OnRotationValueChanged);
    }

    void Update()
    {
        if (_isAutoRotate)
        {
            // X軸回転
            if (Input.GetKeyUp(KeyCode.X))
            {
                _currenRotationAxis = RotationAxis.X;
            }
            // Y軸回転
            else if (Input.GetKeyUp(KeyCode.Y))
            {
                _currenRotationAxis = RotationAxis.Y;
            }
            // Z軸回転
            else if (Input.GetKeyUp(KeyCode.Z))
            {
                _currenRotationAxis = RotationAxis.Z;
            }
            RotationFromAxis(_currenRotationAxis);
        }
        else
        {
            // X軸回転
            if (Input.GetKeyUp(KeyCode.X))
            {
                _currenRotationAxis = RotationAxis.X;
                RotationFromAxis(_currenRotationAxis);
            }
            // Y軸回転
            else if (Input.GetKeyUp(KeyCode.Y))
            {
                _currenRotationAxis = RotationAxis.Y;
                RotationFromAxis(_currenRotationAxis);
            }
            // Z軸回転
            else if (Input.GetKeyUp(KeyCode.Z))
            {
                _currenRotationAxis = RotationAxis.Z;
                RotationFromAxis(_currenRotationAxis);
            }
        }
    }

    private void OnDestroy()
    {
        _isLocalSwitch.onValueChanged.RemoveListener(OnLocalSwitchValueChanged);
        _isStepSwitch.onValueChanged.RemoveListener(OnStepSwitchValueChanged);
        _isAutoRotateSwitch.onValueChanged.RemoveListener(OnAutoRotateValueChanged);
        _valueChangedEvent.RemoveListener(OnRotationValueChanged);
    }
    #endregion

    #region Events
    /// <summary>
    /// ローカル回転設定変更イベント
    /// </summary>
    /// <param name="flg"></param>
    private void OnLocalSwitchValueChanged(bool flg)
    {
        // Debug.Log("Localのスイッチが変更されました");
        _isLocal = flg;
    }

    /// <summary>
    /// 段階的鑑定設定変更イベント
    /// </summary>
    /// <param name="flg"></param>
    private void OnStepSwitchValueChanged(bool flg)
    {
        // Debug.Log("Stepのスイッチが変更されました");
        _isStep = flg;
    }

    private void OnAutoRotateValueChanged(bool flg)
    {
        // Debug.Log("自動回転のスイッチが変更されました");
        _isAutoRotate = flg;
    }

    /// <summary>
    /// 値変更イベント
    /// </summary>
    private void OnRotationValueChanged()
    {
        // Debug.Log("値が変更されました");
        if (_isLocal)
        {
            _textValueX.text = _target.transform.rotation.x.ToString();
            _textValueY.text = _target.transform.rotation.y.ToString();
            _textValueZ.text = _target.transform.rotation.z.ToString();
        }
        else
        {
            _textValueX.text = _target.transform.localRotation.x.ToString();
            _textValueY.text = _target.transform.localRotation.y.ToString();
            _textValueZ.text = _target.transform.localRotation.z.ToString();
        }
    }
    #endregion

    #region Util
    /// <summary>
    /// 現在の設定軸に基づき回転させる
    /// </summary>
    /// <param name="axis">回転軸</param>
    private void RotationFromAxis(RotationAxis axis)
    {
        switch (axis)
        {
            case RotationAxis.X:
                _valueX += _addingvalue;
                RotateByQuaternion(_valueX, 0f, 0f, _target, _isLocal, _isStep);
                break;
            case RotationAxis.Y:
                _valueY += _addingvalue;
                RotateByQuaternion(0f, _valueY, 0f, _target, _isLocal, _isStep);
                break;
            case RotationAxis.Z:
                _valueZ += _addingvalue;
                RotateByQuaternion(0f, 0f, _valueZ, _target, _isLocal, _isStep);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Quaternionによる回転を行う
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="target">動かす対象</param>
    /// <param name="isLocal">ローカル回転がどうか</param>
    /// <param name="isStep">Trueの時は段階的に回転をかける</param>
    /// <param name="isCSharpImplemented">独自実装のQuaternionかどうか</param>
    private void RotateByQuaternion(float x, float y, float z, GameObject target, bool isLocal = false, bool isStep = false, bool isCSharpImplemented = false)
    {
        Quaternion q;

        if (isCSharpImplemented)
        {
            if (isStep)
            {
                // TODO
                q = Quaternion.identity;
            }
            else
            {
                // TODO
                q = Quaternion.identity;
            }
        }
        else
        {
            if (isStep)
            {
                var quaternion1 = Quaternion.Euler(x, 0f, 0f);
                var quaternion2 = Quaternion.Euler(0f, y, 0f);
                var quaternion3 = Quaternion.Euler(0f, 0f, z);
                q = quaternion1 * quaternion2;
                q = q * quaternion3;
            }
            else
            {
                // 内部的にはx->y->zの順で実行しているはず。
                q = Quaternion.Euler(x, y, z);
            }
        }

        // 対象オブジェクトを実際に回転させる。
        if (isLocal)
        {
            // ローカル回転
            target.transform.localRotation = q;
        }
        else
        {
            // グローバル回転
            target.transform.rotation = q;
        }

        // イベント発火
        _valueChangedEvent.Invoke();
    }
    #endregion
}