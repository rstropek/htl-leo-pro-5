export interface QuestionSummaryDto {
    id: number;
    text: string;
    numberOfOptions: number;
}

export interface AddQuestionDto {
    text: string;
    options: AddAnswerOptionDto[];
}

export interface AddAnswerOptionDto {
    text: string;
    isCorrect: boolean;
}

export interface Question
{
    id: number;
    text: string;
    options: AnswerOption[];
}

export interface AnswerOption
{
    id: number;
    text: string;
    isCorrect: boolean;
}

export interface AddQuizDto
{
    numberOfAnsweredQuestions: number;
    numberOfCorrectAnswers: number;
}
