with Ada.Text_IO;
with Ada.Integer_Text_IO;
with Ada.Containers.Indefinite_Doubly_Linked_Lists;
with GNAT.Semaphores;
with Ada.Unchecked_Deallocation;

use Ada.Text_IO;
use Ada.Integer_Text_IO;
use GNAT.Semaphores;

procedure Main is

   package String_Lists is new Ada.Containers.Indefinite_Doubly_Linked_Lists (String);
   use String_Lists;

   Max_Buffer_Size : constant := 5;
   Total_Producers : constant := 3;
   Total_Consumers : constant := 2;

   type Int_Array is array (Positive range <>) of Integer;

   Items_Per_Producer : constant Int_Array := (1 => 5, 2 => 4, 3 => 6);
   Items_Per_Consumer : constant Int_Array := (1 => 7, 2 => 8);

   Buffer : List;

   Access_Sem : Counting_Semaphore (1, Default_Ceiling);
   Full_Sem   : Counting_Semaphore (Max_Buffer_Size, Default_Ceiling);
   Empty_Sem  : Counting_Semaphore (0, Default_Ceiling);

   task type Producer_Task (ID : Positive; Items_To_Produce : Positive);
   task type Consumer_Task (ID : Positive; Items_To_Consume : Positive);

   type Producer_Access is access Producer_Task;
   type Consumer_Access is access Consumer_Task;

   procedure Free_Producer is new Ada.Unchecked_Deallocation (Producer_Task, Producer_Access);
   procedure Free_Consumer is new Ada.Unchecked_Deallocation (Consumer_Task, Consumer_Access);

   task body Producer_Task is
   begin
      for I in 1 .. Items_To_Produce loop
         Full_Sem.Seize;
         Access_Sem.Seize;

         declare
            Item : String := "Item P" & ID'Img & "-" & I'Img;
         begin
            Buffer.Append (Item);
            Put_Line ("[Producer " & ID'Img & "] -> Produced: " & Item);
         end;

         Access_Sem.Release;
         Empty_Sem.Release;
         delay 0.5;
      end loop;
   end Producer_Task;

   task body Consumer_Task is
   begin
      for I in 1 .. Items_To_Consume loop
         Empty_Sem.Seize;
         Access_Sem.Seize;

         declare
            Item : String := First_Element (Buffer);
         begin
            Put_Line ("[Consumer " & ID'Img & "] <- Consumed: " & Item);
         end;

         Buffer.Delete_First;
         Access_Sem.Release;
         Full_Sem.Release;
         delay 0.7;
      end loop;
   end Consumer_Task;

   type Producer_List is array (1 .. Total_Producers) of Producer_Access;
   type Consumer_List is array (1 .. Total_Consumers) of Consumer_Access;

   Producers : Producer_List;
   Consumers : Consumer_List;

begin
   for I in 1 .. Total_Producers loop
      Producers(I) := new Producer_Task (ID => I, Items_To_Produce => Items_Per_Producer(I));
   end loop;

   for I in 1 .. Total_Consumers loop
      Consumers(I) := new Consumer_Task (ID => I, Items_To_Consume => Items_Per_Consumer(I));
   end loop;

   for I in 1 .. Total_Producers loop
      Free_Producer (Producers(I));
   end loop;

   for I in 1 .. Total_Consumers loop
      Free_Consumer (Consumers(I));
   end loop;

end Main;
