//#region Helper functions
function GetRandomInt(max){
    return Math.floor(Math.random() * max);
}

function Sleep(delay){
    let start = new Date().getTime();
    while (new Date().getTime() < start + delay);
}

function Clear(id){
    let div = document.querySelector(id);
    while (div.hasChildNodes()){
        div.firstChild.remove();
    }
}
//#endregion
//#region Construction and Copying of html elements
function MakeH2(text, type, id){
    let h2 = document.createElement('h2');
    h2.textContent = text;
    h2.id = id;
    h2.className = type;
    return h2;
}

function MakeButton(text, type, id){
    let Button = document.createElement('button');
    Button.textContent = text;
    Button.className = type;
    Button.id = id;
    return Button;
}

function MakeImg(src, type, id){
    let img = document.createElement('img');
    img.src = src;
    img.id = id;
    img.className = type;
    return img;
}

function MakeDiv(type, id, ...params){
    let div = document.createElement('div');
    div.className = type;
    div.id = id;
    for (let i = 0; i < params.length; i++)
        div.appendChild(params[i]);
    return div;
}
//#endregion
//#region Challange 1: Age in days
const RESULT_1 = '#result-1';
document.querySelector('#ch-1-start-button').addEventListener('click', AgeInDays);
document.querySelector('#ch-1-clear-button').addEventListener('click', function(){ Clear(RESULT_1) });

function AgeInDays(){
    Clear(RESULT_1);
    let birth = prompt('What year were you born?');
    let result = (2021 - birth) * 365;
    document.querySelector(RESULT_1).appendChild(MakeH2('You are ' + result + ' days old!', undefined, undefined));
}
//#endregion
//#region Challange 2: Cat generator
const RESULT_2 = '#result-2';
document.querySelector('#ch-2-generate-button').addEventListener('click', GenerateCat);
document.querySelector('#ch-2-clear-button').addEventListener('click', function(){ Clear(RESULT_2) });

function GenerateCat(){
    document.querySelector(RESULT_2).appendChild(MakeImg("http://thecatapi.com/api/images/get?format=src&type=gif&size=small", undefined, undefined));
}
//#endregion
//#region Challange 3: Rock Paper Scissors
const RESULT_3 = '#result-3';
const RPS_IMGS = [MakeImg("https://e7.pngegg.com/pngimages/710/76/png-clipart-rarity-animated-film-lucy-van-pelt-rock-angle-rock.png", "RPSimg-clickable", 'ch-3-rock-img'),
                MakeImg("https://images.pond5.com/animated-paper-sheets-illustration-whiteboard-042573484_prevstill.jpeg", "RPSimg-clickable", 'ch-3-paper-img'),
                MakeImg("https://img.favpng.com/17/14/25/scissors-cartoon-clip-art-png-favpng-m62vhTCXqLZ5K3nzma7Nk1VRm.jpg", "RPSimg-clickable", 'ch-3-scissors-img')];
const RESET_BTN = MakeButton('Play again', 'Btn Btn-red', 'ch-3-reset-button');
SetRPSimgs();
document.querySelector('#ch-3-rock-img').addEventListener('click', function(){ RPSround('rock'); });
document.querySelector('#ch-3-paper-img').addEventListener('click', function(){ RPSround('paper'); });
document.querySelector('#ch-3-scissors-img').addEventListener('click', function(){ RPSround('scissors'); });

function SetRPSimgs(){
    let RPSdiv = document.querySelector(RESULT_3);
    for (let i = 0; i < 3; i++)
        RPSdiv.appendChild(RPS_IMGS[i]);
}

function RPSfront_end(yourChoice, botChoice, message){
    let imgs = ["https://e7.pngegg.com/pngimages/710/76/png-clipart-rarity-animated-film-lucy-van-pelt-rock-angle-rock.png",
                "https://images.pond5.com/animated-paper-sheets-illustration-whiteboard-042573484_prevstill.jpeg",
                "https://img.favpng.com/17/14/25/scissors-cartoon-clip-art-png-favpng-m62vhTCXqLZ5K3nzma7Nk1VRm.jpg"];
    Clear(RESULT_3);
    let cls = RESET_BTN.classList[1];
    RESET_BTN.classList.remove(cls);
    RESET_BTN.classList.add("Btn-red");
    let RPSdiv = document.querySelector(RESULT_3);
    RPSdiv.appendChild(MakeDiv("Container", undefined, MakeH2("Your choice", undefined, undefined), MakeImg(imgs[yourChoice], "RPSimg", undefined)));
    RPSdiv.appendChild(MakeDiv("Container", undefined, MakeH2(message.outcomeText, message.type, undefined), RESET_BTN));
    RPSdiv.appendChild(MakeDiv("Container", undefined, MakeH2("Bot choice", undefined, undefined), MakeImg(imgs[botChoice], "RPSimg", undefined)));
    document.querySelector('#ch-3-reset-button').addEventListener('click', function(){ Clear(RESULT_3); SetRPSimgs(); });
}

function DecideWinner(humanChoice, botChoice){
    if (humanChoice == botChoice)
        return 'You Tied!';
    if (humanChoice == (botChoice + 1) % 3)
        return 'You won!';
    return 'You lost!';
}

function RPSround(input){
    
    let ids = ['rock', 'paper', 'scissors'];
    let yourChoice = ids.indexOf(input);
    let botChoice = GetRandomInt(3);
    let outcomeText = DecideWinner(yourChoice, botChoice);
    let type;
    if (outcomeText == 'You won!') type = "Text-green";
    if (outcomeText == 'You lost!') type = "Text-red";
    RPSfront_end(yourChoice, botChoice, {outcomeText, type});
}
//#endregion
//#region Challange 4: Change the color of all buttons
document.querySelector('#ch-3-selection-button').addEventListener('change', function(){ ButtonColorChange(this); });

let allButtons = [].slice.call(document.querySelectorAll('button'));
let originalBtnClasses = [];
for (let i = 0; i < allButtons.length; i++)
    originalBtnClasses.push(allButtons[i].classList[1]);


function Change(SetColor){
    for (let i = 0; i < allButtons.length; i++){
        let cls = allButtons[i].classList[1];
        allButtons[i].classList.remove(cls);
        SetColor(allButtons[i]);
    }
}

function UpdateButtons(){
    let curAllButtons = [].slice.call(document.querySelectorAll('button'));
    for (let i = 0; i < allButtons.length; i++){
        if (!curAllButtons.includes(allButtons[i])){
            allButtons.splice(i);
            originalBtnClasses.splice(i);
        }
    }
    for (let i = 0; i < curAllButtons.length; i++){
        if (!allButtons.includes(curAllButtons[i])){
            allButtons.push(curAllButtons[i]);
            originalBtnClasses.push(curAllButtons[i].classList[1]);
        }
    }
}

function ButtonColorChange(input){
    let allClasses = ["Btn-red", "Btn-blue", "Btn-green", "Btn-yellow"];
    UpdateButtons();
    let value = input.value;
    if (value == 'red'){
        Change(function(btn){
            btn.classList.add("Btn-red");
        });
    }
    else if (value == 'green'){
        Change(function(btn){
            btn.classList.add("Btn-green");
        });
    }
    else if (value == 'random'){
        Change(function(btn){
            let ind = GetRandomInt(4);
            btn.classList.add(allClasses[ind]);
        });
    }
    else{
        Change(function(btn){
            let ind = allButtons.indexOf(btn);
            btn.classList.add(originalBtnClasses[ind]);
        });
    }
}
//#endregion
//#region Challange 5: Blackjack
const YOU = {'scoreSpan': '#your-count', 'div': '#your-cards', 'score': 0};
const DEALER = {'scoreSpan': '#dealers-count', 'div': '#dealers-cards', 'score': 0};
const TABLE = {'win': '#win-count', 'loss': '#loss-count', 'draw': '#draw-count'};
const RESULT_SPAN = '#bj-result';
const SOUNDS = {'hit': new Audio('sounds/swish.m4a'), 'win': new Audio('sounds/cash.mp3'), 'lose': new Audio('sounds/aww.mp3')};
document.querySelector('#ch-5-hit-button').addEventListener('click', function(){ BJhit('human'); });
document.querySelector('#ch-5-stand-button').addEventListener('click', function(){ BJstand('human'); });
document.querySelector('#ch-5-deal-button').addEventListener('click', BJdeal);


let bj_first_turn = 'human';
let finished = false;

function BJReset(){
    document.querySelector(YOU['scoreSpan']).textContent = '0';
    document.querySelector(YOU['scoreSpan']).setAttribute("style", "color:white");
    YOU['score'] = 0;
    document.querySelector(DEALER['scoreSpan']).textContent = '0';
    document.querySelector(DEALER['scoreSpan']).setAttribute("style", "color:white");
    DEALER['score'] = 0;
    let txt = document.querySelector(RESULT_SPAN).textContent;
    let table_spn;
    if (txt == 'You won!')
    table_spn = document.querySelector(TABLE['win']);
    else if (txt == 'You lost!')
    table_spn = document.querySelector(TABLE['loss']);
    else
    table_spn = document.querySelector(TABLE['draw']);
    let cnt = parseInt(table_spn.textContent);
    cnt++;
    table_spn.textContent = cnt;
    document.querySelector(RESULT_SPAN).textContent = 'Let\'s play!';
    document.querySelector(RESULT_SPAN).setAttribute("style", "color:black");
    Clear(YOU['div']);
    Clear(DEALER['div']);
}

function DisplayCard(activePlayer, card){
    const cardImgs = ['images/2.png', 'images/3.png', 'images/4.png', 'images/5.png', 'images/6.png', 'images/7.png', 'images/8.png', 'images/9.png',
                    'images/10.png', 'images/A.png', 'images/J.png', 'images/Q.png', 'images/K.png'];
    document.querySelector(activePlayer['div']).appendChild(MakeImg(cardImgs[card], undefined, undefined));
    SOUNDS['hit'].play();
}

function DisplayWinner(winner){
    let spn = document.querySelector(RESULT_SPAN);
    if (winner == 'human'){
        spn.textContent = 'You won!';
        spn.setAttribute("style", "color:green");
        SOUNDS['win'].play();
    }
    else if (winner == 'bot'){
        spn.textContent = 'You lost!';
        spn.setAttribute("style", "color:red");
        SOUNDS['lose'].play();
    }
    else{
        spn.textContent = 'Draw!';
        spn.setAttribute("style", "color:black");
    }
}

function DisplayScore(activePlayer, score){
    if (score > 21){
        document.querySelector(activePlayer['scoreSpan']).textContent = 'BUST!';
        document.querySelector(activePlayer['scoreSpan']).setAttribute("style", "color:red");
        activePlayer['score'] = -1;
    }
    else{
        document.querySelector(activePlayer['scoreSpan']).textContent = score;
        document.querySelector(activePlayer['scoreSpan']).setAttribute("style", "color:white");
        activePlayer['score'] = score;
    }
}

function BJ_DecideWinner(){
    if (YOU['score'] == DEALER['score'])
        return 'draw';
    if (YOU['score'] == -1)
        return 'bot';
    if (DEALER['score'] == -1)
        return 'human';
    if (YOU['score'] > DEALER['score'])
        return 'human';
    return 'bot';
}

function PlayBot(){

    let average_gain = 7;
    if (bj_first_turn == 'human'){
        if (YOU['score'] == -1){
            BJhit('bot');
            BJstand('bot');
            return;
        }
        while (DEALER['score'] != -1){
            if (DEALER['score'] > YOU['score'] || (DEALER['score'] == YOU['score'] && DEALER['score'] + average_gain >= 21))
                break;
            BJhit('bot');
        }
        BJstand('bot');
        return;
    }

    while (DEALER['score'] != -1){
        if (DEALER['score'] + average_gain >= 21)
            break;
        BJhit('bot');
    }
    BJstand('bot');
}

function BJhit(player){

    if (finished)
        return;
    let active;
    if (player == 'human')
        active = YOU;
    else
        active = DEALER;
    if (active['score'] == -1){
        BJstand(player);
        return;
    }
    let card_id = GetRandomInt(13);
    DisplayCard(active, card_id);
    let value = 10;
    if (card_id < 10) value = card_id + 2;
    let sum = active['score'] + value;
    DisplayScore(active, sum);
    if (sum > 21)
        BJstand(player);
}

function BJstand(player){
    if (finished)
        return;
    if (player != bj_first_turn){
        let winner = BJ_DecideWinner();
        DisplayWinner(winner);
        finished = true;
        return;
    }
    if (player == 'bot')
        return;
    PlayBot();
}

function BJdeal(){
    if (!finished)
        return;
    BJReset();
    bj_first_turn = ['human', 'bot'][GetRandomInt(2)];
    finished = false;
    if (bj_first_turn == 'bot')
        PlayBot();
}
//#endregion