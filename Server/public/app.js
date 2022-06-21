function Api(baseUrl) {
    const ENDPOINTS = {
        GetWord: 'getWord'
    }
    
    this.getRandomWord = async () => {
        const response = await fetch(ENDPOINTS.GetWord)
        const value = await response.json()
        return value.word;
    }    
}
function vm() {
    const onGenerateWord = async (vm,api) => {    
        const word = await api.getRandomWord()
        const wordContainer = vm.getWordContainer()
        wordContainer.innerText = word
    }
    this.getWordContainer = () => document.getElementById("wordContainer")    
    this.getGenerateWordBtn = () => document.getElementById("generateWordBtn")    
    this.api = new Api('')
    this.init = () => {
        const btn = this.getGenerateWordBtn()
        btn.onclick = (e) => onGenerateWord(this,this.api)
    }        
    this.init()    
}
new vm({})