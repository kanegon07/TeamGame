using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DebugMouse : MonoBehaviour {
    private bool isCursorLocked = true; // 現在のカーソルのロック状態を管理

    // 別のスクリプトからいじりたいタイミングがあるので
    // 追加させていただきました
    public void Lock(bool flag) {
        if (flag) {
            LockCursor();
        } else {
            UnlockCursor();
        }
    }

    void Start()
    {
        LockCursor(); // ゲーム開始時にカーソルを非表示にしてロック
    }

    void Update()
    {
        // ウィンドウがアクティブになった場合もカーソルを非表示にする
        if (Application.isFocused && isCursorLocked)
        {
            LockCursor();
        }

        // Pキーでカーソルのロック状態を切り替え
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isCursorLocked)
            {
                UnlockCursor(); // カーソルのロックを解除
            }
            else
            {
                LockCursor(); // カーソルをロック
            }
        }
    }

    // カーソルを画面中央にロックし、非表示にする
    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; // カーソルを中央に固定
        Cursor.visible = false; // カーソルを非表示
        isCursorLocked = true; // 状態を更新
    }

    // カーソルのロックを解除し、表示する
    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None; // カーソルのロックを解除
        Cursor.visible = true; // カーソルを表示
        isCursorLocked = false; // 状態を更新
    }
}
