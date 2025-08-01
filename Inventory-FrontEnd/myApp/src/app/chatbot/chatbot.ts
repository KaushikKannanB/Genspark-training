import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ChatbotModalService } from '../services/chat.service';

@Component({
  selector: 'app-chatbot',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './chatbot.html',
  styleUrl: './chatbot.css'
})
export class Chatbot {
  input: string = '';
  answer:string='';
  @Output() closed = new EventEmitter<void>();
  constructor(private chatbotservice: ChatbotModalService)
  {

  }
  messages = [
    { sender: 'bot', text: 'Hello! How can I assist you with inventory today?' },
    ];

  sendMessage() {
    if (!this.input.trim()) return;

    this.messages.push({ sender: 'user', text: this.input });
    this.chatbotservice.answers(this.input).subscribe({
      next:(data:any)=>{
        this.answer=data;
        console.log(data);
      }
    })
    setTimeout(() => {
      this.messages.push({
        sender: 'bot',
        text: this.answer
      });
    }, 500);
    this.input = '';
  }

  closeChat() {
    this.closed.emit();
  }
}
