#nullable enable
using GameCanvas;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class Game : GameBase
{
    bool isPaused = false;
    int count_before_pause = 0;

    int score = 100;
    int high_score = 0;
    string str = "";
    int gameState = 0;
    int button_x = 450;
    int button_y = 70;  
    int button_w = 80;   
    int button_h = 80;  

    const int BULLET_NUM = 4;
    int[] bullet_x = new int [BULLET_NUM];
    int[] bullet_y = new int [BULLET_NUM];
    int[] bullet_speed = new int [BULLET_NUM];
    int bullet_w = 44;
    int bullet_h = 44;
    int cloud_w = 200;
    int cloud_h = 100;

    int top_cloud_x = 50;
    int top_cloud_y = 300;
    int top_cloud_speed = 9;
    int top_cloud_dir = 1;

    int middle_cloud_dir = 1;
    int middle_cloud_x = 10;
    int middle_cloud_y = 500;
    int middle_cloud_speed = 3;

    int bottom_cloud_x = 350;
    int bottom_cloud_y = 600;
    int bottom_cloud_speed = 7;
    int bottom_cloud_dir = 1;

    int plane_x = 300;
    int plane_y = 0;
    int plane_w = 200;
    int plane_h = 100;
    int plane_dir = 1;
    int plane_speed = 5;

    int player_x = 304;
    int player_y = 813;
    int player_w = 70;
    int player_h = 70;

    int player_dir = 1;
    int player_speed = 3;
    int active_bullet_num = 0;
    int count =0;
    float resetTimer = 0;



    public override void InitGame()
    {
        // 画面２秒長押しでリトライできるように
        resetValue();
    }
    

    public override void UpdateGame()
    {
        // 停止ボタン
        if(gc.GetPointerFrameCount(0) == 1 && gc.GetPointerX(0) > button_x && gc.GetPointerX(0) < button_x + button_w && gc.GetPointerY(0) > button_y && gc.GetPointerY(0) < button_y + button_h){
                isPaused = !isPaused;
                if(isPaused)
                    count_before_pause = count;
                else
                    count = count_before_pause;
        }
        if(isPaused)
        return;
   

        if(gameState == 0){

            //タイトル画面の処理
            if(gc.GetPointerFrameCount(0) ==1 ){
                gameState = 1;
            }

        }

        else if(gameState == 1){
            //ゲーム中の処理
            count++;
            score = 100 - count/60;

            // cloudを左右に動かす処理
            top_cloud_x += top_cloud_dir * top_cloud_speed;
            middle_cloud_x += middle_cloud_dir * middle_cloud_speed;
            bottom_cloud_x += bottom_cloud_dir * bottom_cloud_speed;

            // cloudが画面左右に到達したら向きを反転
            if (top_cloud_x < 0 || top_cloud_x > 564) {
                top_cloud_dir = -top_cloud_dir;
            }            
            if (middle_cloud_x < 0 || middle_cloud_x > 564) {
                middle_cloud_dir = -middle_cloud_dir;
            }
            if (bottom_cloud_x < 0 || bottom_cloud_x > 564) {
                bottom_cloud_dir = -bottom_cloud_dir;
            }


            // planeを左右に動かす処理
            plane_x += plane_dir * plane_speed;
            // planeが画面左右に到達したら向きを反転
            if (plane_x < 0 || plane_x > 564) {
                plane_dir = -plane_dir;
            }

            // ハイスコアよりスコアが小さい時、ハイスコアを更新
            if(score<high_score){
                high_score = score;
            }

            active_bullet_num = 4 + count/300;
            if(active_bullet_num > BULLET_NUM){
                active_bullet_num = BULLET_NUM;
            }
            

            if(gc.GetPointerFrameCount(0) ==1 ){
                player_dir = -player_dir;
                gc.PlaySound(GcSound.Click1);
            }

            player_x += player_dir * player_speed;

            for(int i = 0;i < active_bullet_num; i++ )
            {
                //箱を動かす処理
                bullet_y[i] = bullet_y[i] + bullet_speed[i];

                if(bullet_y[i]> 860){
                    bullet_x[i] = gc.Random(0,530);
                    bullet_y[i] = gc.Random(50,100);
                    bullet_speed[i] = gc.Random(3,6);
                }

                //playerと箱の当り判定
                if (gc.CheckHitRect (
                    player_x,player_y,player_w, player_h,
                    bullet_x[i],bullet_y[i],bullet_w,bullet_h)) {
                        //当たった時の処理
                        gameState = 2;
                        gc.Save("hs",high_score);
                        gc.PlaySound(GcSound.Click2,GcSoundTrack.BGM1,true);
                }
            }

            //playerが画面左右に着いてもgameOverになるように
            if(player_x < 0 || player_x > 564){
                gameState = 2;
                gc.Save("hs",high_score);
                gc.PlaySound(GcSound.Click2,GcSoundTrack.BGM1,true);
            }

            //100秒逃げ切ったらGameClear
            if(score == 0){
                gameState = 3;
            }
        }

        else if(gameState == 2){
            //ゲームオーバー時の処理

            if(gc.GetPointerFrameCount(0) ==1 ){
                gc.StopSound(GcSoundTrack.BGM1);
                resetTimer = 0;
            }
            resetTimer += Time.deltaTime;

            if(resetTimer >= 2) {
                gc.StopSound(GcSoundTrack.BGM1);
                resetValue();
                gameState = 0;
            }
        }

        else if(gameState == 3){
            //ゲームクリア時の処理

            if(gc.GetPointerFrameCount(0) ==1 ){
                gc.StopSound(GcSoundTrack.BGM1);
                resetTimer = 0;
            }
            resetTimer += Time.deltaTime;

            if(resetTimer >= 2) {
                gc.StopSound(GcSoundTrack.BGM1);
                resetValue();
                gameState = 0;
            }
        }        

  
    }

    public override void DrawGame()
    {
        // 画面を白で塗りつぶします
        gc.ClearScreen();
        gc.DrawImage(GcImage.BackGround,0,0);
        gc.SetFontSize(60);

        if(gameState == 0){
            //タイトル画面の処理
            gc.SetColor(0, 0, 0);
            gc.DrawString("エイリアンの",100,250);
            gc.DrawString("100秒逃走",130,350);

            // gc.SetColor(172, 53, 53);
            gc.SetColor(194, 71, 71);
            gc.DrawString("TAP TO PLAY!",100,920);
        }
        else if(gameState == 1){
            //ゲーム中の処理
            gc.DrawImage(GcImage.Alien,player_x,player_y, player_w, player_h);
            

            // 飛行機の向きが逆になったら画像も反転させる
            if (plane_dir == 1) {
                gc.DrawImage(GcImage.RightPlane, plane_x,plane_y,plane_w,plane_h);
            }else{
                gc.DrawImage(GcImage.LeftPlane, plane_x,plane_y,plane_w,plane_h);
            }
            
            for(int i =0 ; i < BULLET_NUM ; i ++ ){
                gc.DrawImage(GcImage.Bullet, bullet_x[i],bullet_y[i],bullet_w,bullet_h);  
            
            }
            gc.SetColor(0, 0, 0);
            gc.DrawString("SCORE:"+score,20,130);
            gc.DrawString("HIGH:"+high_score,20,60);

            gc.DrawImage(GcImage.Cloud,top_cloud_x,top_cloud_y, cloud_w, cloud_h);
            gc.DrawImage(GcImage.Cloud,middle_cloud_x,middle_cloud_y, cloud_w, cloud_h);
            gc.DrawImage(GcImage.Cloud,bottom_cloud_x,bottom_cloud_y, cloud_w, cloud_h);

            gc.DrawImage(GcImage.Pause,button_x,button_y, button_w, button_h);

        }
        else if(gameState == 2){
            //ゲームオーバー時の処理
            gc.SetColor(0, 0, 0);    
            gc.DrawString("SCORE:"+score,20,130);
            gc.DrawString("HIGH:"+high_score,20,60);

            gc.DrawString("GAME OVER",140,400);
            gc.SetColor(32, 52, 103);
            gc.DrawString("長押しでRESTART",60,920);

            gc.DrawString(str,0,300);
    
        }
        else if(gameState == 3){
            //ゲームクリア時の処理
            gc.SetColor(255, 0, 0);
            gc.DrawString("GAME CLEAR!!!",100,400);

            gc.DrawString(str,0,300);
    
        }        
        
    }

    // 画面２秒長押しでリトライできるように        

        void resetValue() {
            score = 0;
            count = 0;
            player_x = 304;
            player_y = 813;
            player_dir = 1;
            active_bullet_num = 0;
            gc.ChangeCanvasSize(564, 1002);
            gc.TryLoad("hs",out high_score);

            for(int i = 0;i < BULLET_NUM; i++ )
            {
                bullet_x[i] = gc.Random(0,530);
                bullet_y[i] = gc.Random(50,100);
                bullet_speed[i] = gc.Random(3,6);
            }

            gameState = 0;
    }
}





