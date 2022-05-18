function Api(baseUrl) {
    const ENDPOINTS = {
        GetWord:`${baseUrl}/getWord`
    }
    return {
        getRandomWord: async () => {
            const response = await fetch(ENDPOINTS.GetWord)
            const value = await response.json()
            return value.word;
        }
    }
}
const onGenerateWord = async (vm,api) => {    
    const word = await api.getRandomWord()
    const wordContainer = vm.getWordContainer()
    wordContainer.innerText = word
}
function vm(params) {
    this.getWordContainer = () => document.getElementById("wordContainer")    
    this.getGenerateWordBtn = () => document.getElementById("generateWordBtn")    
    this.api = Api("http://localhost:8080")
    this.init = () => {
        const btn = this.getGenerateWordBtn()
        btn.onclick = (e) => onGenerateWord(this,this.api)
    }        
    this.init()
    console.log("vm initialized")
}
vm({})