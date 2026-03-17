const int jumpButtonPin = 3;
const int duckButtonPin = 2;

int jumpButtonState = 0;
int lastJumpButtonState = jumpButtonState;
int duckButtonState = 0;
int lastDuckButtonState = duckButtonState;


void setup() {
  // put your setup code here, to run once:
  pinMode(jumpButtonPin, INPUT);
  pinMode(duckButtonPin, INPUT);
  Serial.begin(9600);
  Serial.flush();

}

void loop() {
  // put your main code here, to run repeatedly:
  jumpButtonState = digitalRead(jumpButtonPin);
  duckButtonState = digitalRead(duckButtonPin);

  if(jumpButtonState != lastJumpButtonState){
      lastJumpButtonState = jumpButtonState;
      if(jumpButtonState == HIGH){
        Serial.println("Jump Pressed");
      }else{
        Serial.println("Jump Released");
      }
  }

  

  
  if(duckButtonState != lastDuckButtonState){
      lastDuckButtonState = duckButtonState;
      if(duckButtonState == HIGH){
        Serial.println("Duck Pressed");
      }else{
        Serial.println("Duck Released");
      }
  }
}
